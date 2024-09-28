namespace Caravel.AspNetCore.Endpoint;

public static class WebApplicationExtensions
{
    public static IApplicationBuilder MapEndpointFeatures(this WebApplication app, IEndpointRouteBuilder? routeGroupBuilder = null)
    {
        var endpoints = app.Services.GetRequiredService<IEnumerable<IEndpointFeature>>();

        var builder = routeGroupBuilder ?? app;

        foreach (var endpoint in endpoints)
        {
            endpoint.AddEndpoint(builder);
        }

        return app;
    }
}