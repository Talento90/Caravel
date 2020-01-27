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
                NotFoundException e => new HttpError(HttpStatusCode.NotFound, e, traceId),
                UnauthorizedException e => new HttpError(HttpStatusCode.Unauthorized, e, traceId),
                PermissionException e => new HttpError(HttpStatusCode.Forbidden, e, traceId),
                ValidationException e => new HttpError(HttpStatusCode.BadRequest, e, traceId),
                ConflictException e => new HttpError(HttpStatusCode.Conflict, e, traceId),
                OperationCancelledException e => new HttpError(HttpStatusCode.Accepted, e, traceId),
                OperationCanceledException e => new HttpError(HttpStatusCode.Accepted, new OperationCancelledException(Errors.OperationWasCancelled, e), traceId),
                InvalidOperationException e => new HttpError(HttpStatusCode.BadRequest, new CaravelException(Errors.InvalidOperation, e), traceId),
                CaravelException e => new HttpError(HttpStatusCode.InternalServerError, e, traceId),
                _ => new HttpError(HttpStatusCode.InternalServerError, new CaravelException(Errors.Error, ex), traceId)
            };
        }
    }
}