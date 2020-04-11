using System;
using System.Net;
using System.Threading.Tasks;
using Caravel.AspNetCore.Http;
using Caravel.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Caravel.AspNetCore.Middleware
{
    /// <summary>
    /// ExceptionMiddleware captures and handle all the application exceptions.
    /// It converts the exception into an <see cref="HttpError"/>.
    /// </summary>
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
                var caravelException = ex as CaravelException;
                var severity = caravelException?.Error?.Severity ?? Severity.High;

                switch (severity)
                {
                    case Severity.Low:
                        _logger.LogInformation(ex, "Severity: {severity} Error: {ex.Message}", severity, ex.Message);
                        break;
                    case Severity.Medium:
                        _logger.LogWarning(ex, "Severity: {severity} Error: {ex.Message}", severity, ex.Message);
                        break;
                    case Severity.High:
                        _logger.LogError(ex, "Severity: {severity} Error: {ex.Message}", severity, ex.Message);
                        break;
                }

                context.Response.StatusCode = httpError?.Status ?? (int) HttpStatusCode.InternalServerError;
                await context.Response.WriteJsonAsync(httpError);
            }
        }

        protected virtual HttpError HandleException(HttpContext context, Exception ex)
        {
            return ex switch
            {
                NotFoundException e => new HttpError(context, HttpStatusCode.NotFound, e),
                UnauthorizedException e => new HttpError(context, HttpStatusCode.Unauthorized, e),
                PermissionException e => new HttpError(context, HttpStatusCode.Forbidden, e),
                ValidationException e => new HttpError(context, HttpStatusCode.BadRequest, e).SetErrors(e.Errors),
                ConflictException e => new HttpError(context, HttpStatusCode.Conflict, e),
                OperationCancelledException e => new HttpError(context, HttpStatusCode.Accepted, e),
                OperationCanceledException e => new HttpError(context, HttpStatusCode.Accepted, new OperationCancelledException(Errors.OperationWasCancelled, e)),
                InvalidOperationException e => new HttpError(context, HttpStatusCode.BadRequest, new CaravelException(Errors.InvalidOperation, e)),
                CaravelException e => new HttpError(context, HttpStatusCode.InternalServerError, e),
                _ => new HttpError(context, HttpStatusCode.InternalServerError, new CaravelException(Errors.Error, ex))
            };
        }
    }
}