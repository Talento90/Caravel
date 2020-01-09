using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caravel.AppContext;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Caravel.AspNetCore.Middleware
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<LoggingMiddleware> _logger;
        private readonly LoggingOptions _options;
        private readonly IAppContext _context;

        public LoggingMiddleware(RequestDelegate next, ILogger<LoggingMiddleware> logger,
            IOptions<LoggingOptions> options, IAppContext context)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _next = next ?? throw new ArgumentNullException(nameof(next));
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _options = options == null ? new LoggingOptions() : options.Value;
        }

        public async Task Invoke(HttpContext context)
        {
            var request = context.Request;

            if (_options.PathsToIgnore.Any(p => request.Path.Value.ToLower().StartsWith(p)) || request.Path == "/")
            {
                await _next(context);
                return;
            }

            var cid = _context.Context.TraceId;
            var uid = _context.Context.UserId;

            using (_logger.BeginScope("{cid} {uid}", cid, uid))
            {
                await LogRequest(request);

                var start = Stopwatch.StartNew();

                if (_options.EnableLogBody)
                {
                    var originalBodyStream = context.Response.Body;

                    await using var responseBody = new MemoryStream();

                    context.Response.Body = responseBody;

                    await _next(context);

                    start.Stop();

                    context.Response.Body.Seek(0, SeekOrigin.Begin);
                    await LogResponseWithBody(context.Response, start.Elapsed);
                    context.Response.Body.Seek(0, SeekOrigin.Begin);

                    await responseBody.CopyToAsync(originalBodyStream);
                }
                else
                {
                    await _next(context);

                    start.Stop();

                    LogResponseWithoutBody(context.Response, start.Elapsed);
                }
            }
        }


        private async Task LogRequest(HttpRequest request)
        {
            request.EnableBuffering();

            var body = string.Empty;
            var headers = request.Headers.Where(h => h.Key != "Authorization");

            if (_options.HeadersToLog.Any())
            {
                headers = headers.Where(h => _options.HeadersToLog.Contains(h.Key));
            }

            var containsFiles = request.HasFormContentType && request.Form.Files.Any();

            if (!containsFiles && _options.EnableLogBody)
            {
                var buffer = new byte[Convert.ToInt32(request.ContentLength)];
                await request.Body.ReadAsync(buffer, 0, buffer.Length);
                body = Encoding.UTF8.GetString(buffer);
                request.Body.Position = 0;
            }


            if (ShouldRedactBody(request))
            {
                _logger.LogInformation("Request {method} {uri} [redacted] {headers}",
                    request.Method,
                    request.GetDisplayUrl(),
                    headers
                );
            }
            else
            {
                _logger.LogInformation("Request {method} {uri} {body} {headers}",
                    request.Method,
                    request.GetDisplayUrl(),
                    body,
                    headers
                );
            }
        }

        private void LogResponseWithoutBody(HttpResponse response, TimeSpan duration)
        {
            _logger.Log(LogLevel.Information, "Response {statusCode} {method} {uri} {duration:0.000}ms",
                response.StatusCode,
                response.HttpContext.Request.Method,
                response.HttpContext.Request.GetDisplayUrl(),
                duration.TotalMilliseconds
            );
        }


        private async Task LogResponseWithBody(HttpResponse response, TimeSpan duration)
        {
            var body = string.Empty;

            if (response.StatusCode >= 400)
            {
                body = await new StreamReader(response.Body).ReadToEndAsync();
            }

            _logger.Log(LogLevel.Information, "Response {statusCode} {method} {uri} {body} {duration:0.000}ms",
                response.StatusCode,
                response.HttpContext.Request.Method,
                response.HttpContext.Request.GetDisplayUrl(),
                body,
                duration.TotalMilliseconds
            );
        }

        private bool ShouldRedactBody(HttpRequest request)
            => _options.PathsToRedact.Any(p => request.Path.Value.ToLower().StartsWith(p));
    }
}