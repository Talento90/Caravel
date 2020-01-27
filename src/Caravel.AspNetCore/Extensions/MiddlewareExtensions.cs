using Caravel.AspNetCore.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Options;

namespace Caravel.AspNetCore.Extensions
{
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseAppVersion(this IApplicationBuilder builder, string path = "/api/version")
        {
            return builder.Map(path, b => b.UseMiddleware<AppVersionMiddleware>());
        }
        
        public static IApplicationBuilder UseLogging(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<LoggingMiddleware>(Options.Create(new LoggingOptions()));
        }
        
        public static IApplicationBuilder UseTraceId(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<TraceIdMiddleware>(Options.Create(new TraceIdOptions()));
        }
        
        public static IApplicationBuilder UseAppContextEnricher(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<AppContextEnricherMiddleware>();
        }
    }
}