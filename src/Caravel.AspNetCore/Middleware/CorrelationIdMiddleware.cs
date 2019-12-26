using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Caravel.AppContext;
using Caravel.AspNetCore.Http;
using Microsoft.AspNetCore.Http;

namespace Caravel.AspNetCore.Middleware
{
    public class CorrelationIdMiddleware
    {
        public const string CorrelationIdHeader = "X-Correlation-Id";
        private readonly RequestDelegate _next;
        private readonly IAppContext _context;

        public CorrelationIdMiddleware(RequestDelegate next, IAppContext context)
        {
            _next = next;
            _context = context;
        }

        public async Task Invoke(HttpContext context)
        {
            var cidHeader = context.Request.Headers.ContainsKey(CorrelationIdHeader);

            if (cidHeader)
            {
                context.TraceIdentifier = context.Request.Headers[CorrelationIdHeader];
            }
            else
            {
                context.TraceIdentifier = Guid.NewGuid().ToString();
            }

            context.Response.OnStarting(() =>
            {
                context.Response.Headers.TryAdd(CorrelationIdHeader, new[] {context.TraceIdentifier});

                return Task.CompletedTask;
            });
            
            
            _context.Context = new AppContext.AppContext(context.TraceIdentifier, context.User.Id());
            
            await _next(context);
        }
    }
}