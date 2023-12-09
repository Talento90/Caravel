using Caravel.ApplicationContext;

namespace Caravel.AspNetCore.Middleware;

/// <summary>
/// ApplicationContextEnrichMiddleware creates and set the <see cref="ApplicationContext"/>.
/// This middleware is required in order to use the <see cref="ApplicationContext"/> in the application.
/// </summary>
public class ApplicationContextEnrichMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IApplicationContextAccessor _contextAccessor;

    public ApplicationContextEnrichMiddleware(RequestDelegate next, IApplicationContextAccessor contextAccessor)
    {
        _next = next;
        _contextAccessor = contextAccessor;
    }

    public async Task Invoke(HttpContext context)
    {
        _contextAccessor.Context = new ApplicationContext.ApplicationContext(context.User);

        await _next(context);
    }
}