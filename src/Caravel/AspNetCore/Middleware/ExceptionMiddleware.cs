using System;
using System.Net;
using System.Threading.Tasks;
using Caravel.Exceptions;
using Caravel.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Caravel.AspNetCore.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;

        public ExceptionMiddleware(RequestDelegate next, ILoggerFactory loggerFactory)
        {
            _next = next;
            _logger = loggerFactory.CreateLogger<ExceptionMiddleware>();
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

                if (httpError.Status == null || httpError.Status.Value == (int) HttpStatusCode.InternalServerError)
                {
                    _logger.LogError(ex, ex.Message);
                }

                context.Response.StatusCode = httpError.Status ?? (int) HttpStatusCode.InternalServerError;
                context.Response.WriteJson(httpError);
            }
        }

        protected virtual HttpError HandleException(HttpContext context, Exception ex)
        {
            switch (ex)
            {
                case NotFoundException e:
                    return new HttpError("Not Found", HttpStatusCode.NotFound, e, context.TraceIdentifier);
                case UnauthorizedException e:
                    return new HttpError("Unauthorized", HttpStatusCode.Unauthorized, e, context.TraceIdentifier);
                case PermissionException e:
                    return new HttpError("Forbidden", HttpStatusCode.Forbidden, e, context.TraceIdentifier);
                case ValidationException e:
                    return new HttpError("Bad Request", HttpStatusCode.BadRequest, e, context.TraceIdentifier);
                case ConflictException e:
                    return new HttpError("Conflict", HttpStatusCode.Conflict, e, context.TraceIdentifier);
                case OperationCancelledException e:
                    return new HttpError("Conflict", HttpStatusCode.Accepted, e, context.TraceIdentifier);
                case System.OperationCanceledException e:
                    return new HttpError("Request was cancelled", HttpStatusCode.Accepted, new OperationCancelledException(Errors.OperationWasCancelled, e), context.TraceIdentifier);
                case InvalidOperationException e:
                    return new HttpError("Invalid Operation Error", HttpStatusCode.BadRequest, new CaravelException(Errors.InvalidOperation, e), context.TraceIdentifier);
                default:
                    return new HttpError(
                        "Internal Server Error",
                        HttpStatusCode.InternalServerError,
                        new CaravelException(Errors.Error, ex),
                        context.TraceIdentifier
                    );
            }
        }
    }
}