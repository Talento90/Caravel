using Caravel.AspNetCore.Http;
using Caravel.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Caravel.AspNetCore.Middleware;

public class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken ct)
    {
        
        _logger.LogError(exception, "Error: {ex.Message}", exception.Message);
        var error = HandleException(exception);
        
        httpContext.Response.StatusCode = error.Status ?? StatusCodes.Status500InternalServerError;
        await httpContext.Response.WriteAsJsonAsync(error, ct);

        return true;
    }

    private static HttpError<> HandleException(Exception ex)
    {
        return ex switch
        {
            ValidationException validationException => new ProblemDetails
            {
                Title = "Invalid Request",
                Status = StatusCodes.Status400BadRequest,
                Detail = "Some problem occured. If it keeps happening, please contact support.",
                Extensions = new Dictionary<string, object?>
                {
                    { "errors", validationException.Errors },
                    { "code", "invalid_fields"}
                }
            },
            _ => new ProblemDetails()
            {
                Title = "Server Error",
                Status = StatusCodes.Status500InternalServerError,
                Detail = "Some problem occured. If it keeps happening, please contact support.",
                Extensions = new Dictionary<string, object?>
                {
                    { "code", "internal_error"}
                }
            }
        };
    }
}