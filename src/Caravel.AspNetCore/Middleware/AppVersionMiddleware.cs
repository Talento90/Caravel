using System.Net;
using System.Threading.Tasks;
using Caravel.AspNetCore.Http;
using Microsoft.AspNetCore.Http;

namespace Caravel.AspNetCore.Middleware
{
    public class AppVersionMiddleware
    {
        private readonly RequestDelegate _next;

        public AppVersionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var appVersion = new AppVersion(Env.GetAppVersion());
            context.Response.StatusCode = (int) HttpStatusCode.OK;
            await context.Response.WriteJsonAsync(appVersion);
        }

        public class AppVersion
        {
            public string Version { get; }
            
            public AppVersion(string version)
            {
                Version = version;
            }
        }
    }
}