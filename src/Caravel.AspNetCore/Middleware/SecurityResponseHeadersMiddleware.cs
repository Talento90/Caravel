using Microsoft.Net.Http.Headers;

namespace Caravel.AspNetCore.Middleware;

public class SecurityResponseHeadersMiddleware
{
    private readonly RequestDelegate _next;

    public SecurityResponseHeadersMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public Task Invoke(HttpContext context)
    {
        // Never allow resource to be loaded in frames
        context.Response.Headers.Append(HeaderNames.XFrameOptions, "deny");
        // Always follow Content-Type returned in response
        context.Response.Headers.Append(CaravelHeaderNames.XContentTypeOptions, "nosniff");
        // Detect and block reflected cross-site scripting attacks
        context.Response.Headers.Append(CaravelHeaderNames.XXssProtection, "1; mode=block");
        // Prevent sensitive info from being leaked to external domains
        context.Response.Headers.Append(CaravelHeaderNames.ReferrerPolicy, "no-referrer");
        // Prevent browser to cache HTTP content
        context.Response.Headers.Append(HeaderNames.CacheControl, "no-store");
        // Ensure browsers automatically load website over HTTPS
        context.Response.Headers.Append(HeaderNames.StrictTransportSecurity, "max-age=63072000; includeSubDomains");
        // Disable resource source fallback and content loading in frames
        context.Response.Headers.Append(HeaderNames.ContentSecurityPolicy, "default-src 'none'; frame-ancestors 'none'");

        return _next.Invoke(context);
    }
}