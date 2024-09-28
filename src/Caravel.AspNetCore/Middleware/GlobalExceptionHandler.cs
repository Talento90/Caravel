using System.Diagnostics;
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
            Activity.Current?.SetTag(ObservabilityTags.ErrorType, caravelException.Error.Type);
            Activity.Current?.SetTag(ObservabilityTags.ErrorCode, caravelException.Error.Code);

            var logLevel = caravelException.Error.Severity switch
            {
                ErrorSeverity.Low => LogLevel.Information,
                ErrorSeverity.Medium => LogLevel.Warning,
                ErrorSeverity.High => LogLevel.Information,
                _ => LogLevel.Critical
            };
            
            _logger.Log(logLevel, exception, "Exception: {Message}", exception.Message);    
        }
        else
        {
            Activity.Current?.SetTag(ObservabilityTags.ErrorType, "unhandled");
            Activity.Current?.SetTag(ObservabilityTags.ErrorCode, "unhandled");

            _logger.LogError(exception, "Unknown Exception: {Message}", exception.Message);    
        }
        
        var error = HandleException(exception);
        
        httpContext.Response.StatusCode = error.Status ?? StatusCodes.Status500InternalServerError;
        await httpContext.Response.WriteAsJsonAsync(error, ct);

        return true;
    }

    private static ApiProblemDetails HandleException(Exception ex)
    {
        return ex switch
        {
            CaravelException caravelException => new ApiProblemDetails(caravelException.Error),
            _ => new ApiProblemDetails(
                Error.Internal(
                    "internal",
                    "Server Error",
                    "Some problem occured. If it keeps happening, please contact support.",
                    ErrorSeverity.High
                ))
        };
    }
}