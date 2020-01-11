using System;
using System.Net;
using System.Threading.Tasks;
using Caravel.AspNetCore.Http;
using Caravel.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Caravel.AspNetCore.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                var httpError = HandleException(context, ex);
                
                switch (httpError?.Exception?.Error?.Severity)
                {
                    case Severity.Low:
                        _logger.LogInformation(ex, ex.Message);
                        break;
                    case Severity.Medium:
                        _logger.LogWarning(ex, ex.Message);
                        break;
                    default:
                        _logger.LogError(ex, ex.Message);
                        break;
                }

                context.Response.StatusCode = httpError?.Status ?? (int) HttpStatusCode.InternalServerError;
                await context.Response.WriteJsonAsync(httpError);
            }
        }

        protected virtual HttpError HandleException(HttpContext context, Exception ex)
        {
            var traceId = context.TraceIdentifier;

            return ex switch
            {
                NotFoundException e => new HttpError("Not Found", HttpStatusCode.NotFound, e, traceId),
                UnauthorizedException e => new HttpError("Unauthorized", HttpStatusCode.Unauthorized, e, traceId),
                PermissionException e => new HttpError("Forbidden", HttpStatusCode.Forbidden, e, traceId),
                ValidationException e => new HttpError("Bad Request", HttpStatusCode.BadRequest, e, traceId),
                ConflictException e => new HttpError("Conflict", HttpStatusCode.Conflict, e, traceId),
                OperationCancelledException e => new HttpError("Operation was cancelled", HttpStatusCode.Accepted, e, traceId),
                OperationCanceledException e => new HttpError("Request was cancelled", HttpStatusCode.Accepted, new OperationCancelledException(Errors.OperationWasCancelled, e), traceId),
                InvalidOperationException e => new HttpError("Invalid Operation Error", HttpStatusCode.BadRequest, new CaravelException(Errors.InvalidOperation, e), traceId),
                CaravelException e => new HttpError("Internal Server Error", HttpStatusCode.InternalServerError, e, traceId),
                _ => new HttpError("Internal Server Error", HttpStatusCode.InternalServerError, new CaravelException(Errors.Error, ex), traceId)
            };
        }
    }
}