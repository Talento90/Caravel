using System.Collections.Generic;
using System.Linq;

namespace Caravel.AspNetCore.Middleware
{
    public class LoggingMiddlewareSettings
    {
        public IEnumerable<string> PathsToIgnore { get; }
        public IEnumerable<string> PathsToRedact { get; }
        public bool EnableLogBody { get; }

        public LoggingMiddlewareSettings(IEnumerable<string> pathsToIgnore, IEnumerable<string> pathsToRedact,
            bool enableLogBody = false)
        {
            PathsToIgnore = pathsToIgnore ?? Enumerable.Empty<string>();
            PathsToRedact = pathsToRedact ?? Enumerable.Empty<string>();;
            EnableLogBody = enableLogBody;
        }
        
        public static LoggingMiddlewareSettings DefaultSettings()
        {
            return new LoggingMiddlewareSettings(new[] {"/api-docs", "/swagger", "/health", "/api/version"},
                Enumerable.Empty<string>());
        }
    }
}