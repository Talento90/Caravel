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
    /// <summary>
    /// LoggingMiddleware logs the HTTP request and responses.
    /// Check the <see cref="LoggingSettings"/> to configure the logging.
    /// </summary>
    public class LoggingMiddleware
    {
        private const string DynamicPath = "{*}";

        private readonly RequestDelegate _next;
        private readonly ILogger<LoggingMiddleware> _logger;
        private readonly LoggingSettings _settings;
        private readonly IAppContextAccessor _contextAccessor;
        
        public LoggingMiddleware(RequestDelegate next, ILogger<LoggingMiddleware> logger,
            IOptions<LoggingSettings> options, IAppContextAccessor contextAccessor)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _next = next ?? throw new ArgumentNullException(nameof(next));
            _contextAccessor = contextAccessor ?? throw new ArgumentNullException(nameof(contextAccessor));
            _settings = options == null ? new LoggingSettings() : options.Value;
        }

        public async Task Invoke(HttpContext context)
        {
            var request = context.Request;

            if (_settings.PathsToIgnore.Any(p => request.Path.Value.ToLower().StartsWith(p)) || request.Path == "/")
            {
                await _next(context);
                return;
            }

            var traceId = _contextAccessor.Context.TraceId;
            var uid = _contextAccessor.Context.UserId;

            using (_logger.BeginScope("{traceId} {uid}", traceId, uid))
            {
                await LogRequest(request);

                var start = Stopwatch.StartNew();

                if (_settings.EnableLogBody)
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

            if (_settings.HeadersToLog.Any())
            {
                headers = headers.Where(h => _settings.HeadersToLog.Contains(h.Key));
            }

            var containsFiles = request.HasFormContentType && request.Form.Files.Any();

            if (!containsFiles && _settings.EnableLogBody)
            {
                body = await ReadBodyAsync(request.Body);
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
            var body = await ReadBodyAsync(response.Body);
  
            _logger.Log(LogLevel.Information, "Response {statusCode} {method} {uri} {body} {duration:0.000}ms",
                response.StatusCode,
                response.HttpContext.Request.Method,
                response.HttpContext.Request.GetDisplayUrl(),
                body,
                duration.TotalMilliseconds
            );
        }

        private static async Task<string> ReadBodyAsync(Stream stream)
        {
            stream.Seek(0, SeekOrigin.Begin);
            using var reader = new StreamReader(stream, Encoding.UTF8,detectEncodingFromByteOrderMarks: false, leaveOpen: true);
            var body = await reader.ReadToEndAsync();
            stream.Seek(0, SeekOrigin.Begin);

            return body;
        }

        private bool ShouldRedactBody(HttpRequest request)
        {
            foreach (var redactPath in _settings.PathsToRedact)
            {
                var normalizePath = request.Path.Value.ToLower();
                var isDynamicPath = redactPath.Contains(DynamicPath);

                if (!isDynamicPath)
                {
                    return normalizePath.StartsWith(redactPath);
                }
                
                var dynamicParts = redactPath.Split(DynamicPath);
                
                return dynamicParts.All(d => normalizePath.Contains(d));
            }

            return false;
        }
    }
}