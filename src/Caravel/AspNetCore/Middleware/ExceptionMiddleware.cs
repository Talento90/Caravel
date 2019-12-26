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
            var cid = context.TraceIdentifier;
            
            return ex switch
            {
                NotFoundException e => new HttpError("Not Found", HttpStatusCode.NotFound, e, cid),
                UnauthorizedException e => new HttpError("Unauthorized", HttpStatusCode.Unauthorized, e, cid),
                PermissionException e => new HttpError("Forbidden", HttpStatusCode.Forbidden, e,cid),
                ValidationException e => new HttpError("Bad Request", HttpStatusCode.BadRequest, e,cid),
                ConflictException e => new HttpError("Conflict", HttpStatusCode.Conflict, e, cid),
                OperationCancelledException e => new HttpError("Operation was cancelled", HttpStatusCode.Accepted, e,cid),
                OperationCanceledException e => new HttpError("Request was cancelled", HttpStatusCode.Accepted, new OperationCancelledException(Errors.OperationWasCancelled, e), cid),
                InvalidOperationException e => new HttpError("Invalid Operation Error", HttpStatusCode.BadRequest, new CaravelException(Errors.InvalidOperation, e), cid),
                CaravelException e => new HttpError("Internal Server Error", HttpStatusCode.InternalServerError, e,cid),
                _ => new HttpError("Internal Server Error", HttpStatusCode.InternalServerError, new CaravelException(Errors.Error, ex), cid)
            };
        }
    }
}