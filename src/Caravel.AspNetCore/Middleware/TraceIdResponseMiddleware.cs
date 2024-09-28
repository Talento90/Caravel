using System.Diagnostics;
using System.Security.Claims;
using Caravel.Security;

namespace Caravel.AspNetCore.Middleware;

public class TraceIdResponseMiddleware
{
    private readonly RequestDelegate _next;

    public TraceIdResponseMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        context.Response.Headers.Append(CaravelHeaderNames.TraceId, Activity.Current?.TraceId.ToString());

        await _next(context);
    }
}