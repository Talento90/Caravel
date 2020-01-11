using System.Threading.Tasks;
using Caravel.AppContext;
using Caravel.AspNetCore.Http;
using Microsoft.AspNetCore.Http;

namespace Caravel.AspNetCore.Middleware
{
    public class AppContextEnricherMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IAppContextAccessor _contextAccessor;

        public AppContextEnricherMiddleware(RequestDelegate next, IAppContextAccessor contextAccessor)
        {
            _next = next;
            _contextAccessor = contextAccessor;
        }

        public async Task Invoke(HttpContext context)
        {
            _contextAccessor.Context = new AppContext.AppContext(context.TraceIdentifier, context.User.Id());

            await _next(context);
        }
    }
}