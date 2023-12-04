using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Caravel.ApplicationContext;
using Caravel.AspNetCore.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace Caravel.AspNetCore.Middleware
{
    /// <summary>
    /// AppContextEnricherMiddleware creates and set the <see cref="ApplicationContext"/>.
    /// This middleware is required in order to use the <see cref="ApplicationContext"/> in the application.
    /// </summary>
    public class AppContextEnricherMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IApplicationContextAccessor _contextAccessor;
        private readonly AppContextEnricherSettings _settings;
        public AppContextEnricherMiddleware(RequestDelegate next, IApplicationContextAccessor contextAccessor, IOptions<AppContextEnricherSettings> options)
        {
            _next = next;
            _contextAccessor = contextAccessor;
            _settings = options.Value;
        }

        public async Task Invoke(HttpContext context)
        {
            _contextAccessor.Context = new ApplicationContext.ApplicationContext(
                context.TraceIdentifier,
                context.User.Id(),
                context.User.TenantId());
            
            foreach (var claim in _settings.Claims)
            {
                var jwtClaim = context.User?.Claims?.FirstOrDefault(c => c.Type == claim);

                if (jwtClaim != null)
                {
                    _contextAccessor.Context.Data.Add(jwtClaim.Type, jwtClaim.Value);
                }
            }

            await _next(context);
        }
    }
}