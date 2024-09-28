using System.Diagnostics;
using System.Security.Claims;
using Caravel.Security;

namespace Caravel.AspNetCore.Middleware;

public class ActivityEnrichingMiddleware
{
    private readonly RequestDelegate _next;

    public ActivityEnrichingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        var scope = string.Join(",", context.User.FindAll(c => c.Type == "scope").Select(c => c.Value).ToArray());

        Activity.Current?.SetTag(ObservabilityTags.Scope, scope);
        Activity.Current?.SetTag(ObservabilityTags.UserId, context.User.UserId());
        Activity.Current?.SetTag(ObservabilityTags.TenantId, context.User.TenantId());
        Activity.Current?.SetTag(ObservabilityTags.TraceId, Activity.Current.TraceId.ToString());

        context.Response.Headers.Append(CaravelHeaderNames.TraceId, Activity.Current?.TraceId.ToString());

        await _next(context);
    }
}