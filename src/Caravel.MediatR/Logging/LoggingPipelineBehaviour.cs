using System.Diagnostics;
using Caravel.Errors;
using Caravel.Functional;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Caravel.MediatR.Logging;

public sealed class LoggingPipelineBehaviour<TRequest, TResponse>(
    ILogger<LoggingPipelineBehaviour<TRequest, TResponse>> logger)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : class
    where TResponse : Result
{
    private readonly Stopwatch _timer = new();

    private static LogLevel GetLogLevel(ErrorSeverity severity)
    {
        return severity switch
        {
            ErrorSeverity.Critical => LogLevel.Critical,
            ErrorSeverity.High => LogLevel.Error,
            ErrorSeverity.Medium => LogLevel.Warning,
            _ => LogLevel.Debug
        };
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var requestName = typeof(TRequest);

        _timer.Start();
        logger.LogInformation("Starting request {RequestName} {@DateTimeUtc}",
            requestName,
            DateTime.UtcNow);

        var response = await next();
        _timer.Stop();

        if (!response.IsSuccess)
        {
            var logLevel = GetLogLevel(response.Error.Severity);
            logger.Log(logLevel, "Request failed {RequestName} with {ErrorCode} - {Message}",
                requestName,
                response.Error.Code,
                response.Error.Message);
        }

        logger.LogInformation("Request completed {RequestName} in {Duration} ms", requestName, _timer.ElapsedMilliseconds);

        return response;
    }
}