using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Caravel.AppContext;
using Caravel.AspNetCore.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace Caravel.AspNetCore.Middleware
{
    public class TraceIdMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IAppContext _context;
        private readonly TraceIdOptions _options;

        public TraceIdMiddleware(RequestDelegate next, IOptions<TraceIdOptions> options, IAppContext context)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _options = options == null ? new TraceIdOptions() : options.Value;
        }

        public async Task Invoke(HttpContext context)
        {
            context.TraceIdentifier = context.Request.Headers.ContainsKey(_options.Header)
                ? context.Request.Headers[_options.Header].ToString()
                : Guid.NewGuid().ToString();

            if (_options.IncludeInResponse)
            {
                context.Response.OnStarting(() =>
                {
                    context.Response.Headers.TryAdd(_options.Header, new[] {context.TraceIdentifier});

                    return Task.CompletedTask;
                });
            }

            _context.Context = new AppContext.AppContext(context.TraceIdentifier, context.User.Id());

            await _next(context);
        }
    }
}