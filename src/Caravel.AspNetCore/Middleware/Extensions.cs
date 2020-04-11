using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Options;

namespace Caravel.AspNetCore.Middleware
{
    /// <summary>
    /// Extensions methods to register the middleware.
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Register the AppVersionMiddleware.
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseAppVersion(this IApplicationBuilder builder, string path = "/api/version")
        {
            return builder.Map(path, b => b.UseMiddleware<AppVersionMiddleware>());
        }
        
        /// <summary>
        /// Register the LoggingMiddleware.
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseLogging(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<LoggingMiddleware>(Options.Create(new LoggingOptions()));
        }
        
        /// <summary>
        /// Register the TraceIdMiddleware.
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseTraceId(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<TraceIdMiddleware>(Options.Create(new TraceIdOptions()));
        }
        
        /// <summary>
        /// Register the AppContextEnricherMiddleware.
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseAppContextEnricher(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<AppContextEnricherMiddleware>();
        }
    }
}