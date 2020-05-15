using System;
using System.Net;
using System.Threading.Tasks;
using Caravel.AspNetCore.Http;
using Caravel.Errors;
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
                NotFoundException e => new HttpError(context, HttpStatusCode.NotFound, new Error("not_found", "The resource does not exists.")),
                UnauthorizedException e => new HttpError(context, HttpStatusCode.Unauthorized, new Error("unauthorized", "User is not authenticated.")),
                PermissionException e => new HttpError(context, HttpStatusCode.Forbidden, new Error("permission", "Insufficient permissions to execute the operation.")),
                ValidationException e => new HttpError(context, HttpStatusCode.BadRequest, new Error("validation", "Validation error.")).SetErrors(e.Errors),
                ConflictException e => new HttpError(context, HttpStatusCode.Conflict, new Error("conflict",  "Conflict executing the operation.")),
                _ => new HttpError(context, HttpStatusCode.InternalServerError, new Error("internal", "Internal error server."))
            };
        }
    }
}