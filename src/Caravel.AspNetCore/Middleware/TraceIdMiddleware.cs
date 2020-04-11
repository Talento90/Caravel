using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Caravel.AppContext;
using Caravel.AspNetCore.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace Caravel.AspNetCore.Middleware
{
    /// <summary>
    /// TraceIdMiddleware reads a specific header in order to inject the TraceId/CorrelationId.
    /// </summary>
    public class TraceIdMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IAppContextAccessor _contextAccessor;
        private readonly TraceIdOptions _options;

        public TraceIdMiddleware(RequestDelegate next, IOptions<TraceIdOptions> options, IAppContextAccessor contextAccessor)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
            _contextAccessor = contextAccessor ?? throw new ArgumentNullException(nameof(contextAccessor));
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

            _contextAccessor.Context = new AppContext.AppContext(context.TraceIdentifier, context.User.Id());

            await _next(context);
        }
    }
}