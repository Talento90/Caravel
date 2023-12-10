using Caravel.AspNetCore.Http;
using Caravel.Errors;
using Caravel.Exceptions;
using Microsoft.AspNetCore.Diagnostics;

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
        if (exception is CaravelException caravelException)
        {
            var logLevel = caravelException.Error.Severity switch
            {
                ErrorSeverity.Low => LogLevel.Information,
                ErrorSeverity.Medium => LogLevel.Warning,
                ErrorSeverity.High => LogLevel.Information,
                _ => LogLevel.Critical
            };
            
            _logger.Log(logLevel, exception, "Exception: {ex.Message}", exception.Message);    
        }
        else
        {
            _logger.LogError(exception, "Unknown Exception: {ex.Message}", exception.Message);    
        }
        
        var error = HandleException(exception);
        
        httpContext.Response.StatusCode = error.Status ?? StatusCodes.Status500InternalServerError;
        await httpContext.Response.WriteAsJsonAsync(error, ct);

        return true;
    }

    private static HttpError HandleException(Exception ex)
    {
        return ex switch
        {
            CaravelException caravelException => new HttpError(caravelException.Error),
            _ => new HttpError(
                Error.Internal(
                    "internal_server",
                    "Server Error",
                    "Some problem occured. If it keeps happening, please contact support.",
                    ErrorSeverity.High
                ))
        };
    }
}