using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Caravel.AspNetCore.Extensions
{
    public static class EndpointRouteExtensions
    {
        public static IEndpointConventionBuilder MapServiceVersion(this IEndpointRouteBuilder builder, string pattern = "/api/version")
        {
            return builder.MapGet(pattern,
                async httpContext =>
                {
                    await httpContext.Response.WriteAsync(Env.GetAppVersion());
                });
        }
    }
}