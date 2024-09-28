using System.Diagnostics;
using Caravel.Errors;
using Caravel.Functional;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Caravel.MediatR.Logging;

public class LoggingRequestBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, Result<TResponse>>
    where TRequest : IRequest<TResponse>
{
    private readonly Stopwatch _timer;
    private readonly ILogger<TRequest> _logger;

    public LoggingRequestBehaviour(ILogger<TRequest> logger)
    {
        _timer = new Stopwatch();
        _logger = logger;
    }

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

    public async Task<Result<TResponse>> Handle(TRequest request, RequestHandlerDelegate<Result<TResponse>> next,
        CancellationToken cancellationToken)
    {
        var requestName = typeof(TRequest);
        
        _timer.Start();
        _logger.LogInformation("Starting request {RequestName} {@DateTimeUtc}", 
            requestName, 
            DateTime.UtcNow);
        
        var response = await next();
        _timer.Stop();

        if (!response.IsSuccess)
        {
            var logLevel = GetLogLevel(response.Error.Severity);
            _logger.Log(logLevel, "Request failed {RequestName} with {ErrorCode} - {Message}",
                requestName,
                response.Error.Code,
                response.Error.Message);
        }
        
        _logger.LogInformation("Request completed {RequestName} in {duration} ms", 
            requestName,
            _timer.ElapsedMilliseconds);

        return response;
    }
}