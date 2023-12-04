using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Caravel.ApplicationContext;
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
        private readonly IApplicationContextAccessor _contextAccessor;
        private readonly TraceIdSettings _settings;

        public TraceIdMiddleware(RequestDelegate next, IOptions<TraceIdSettings> options, IApplicationContextAccessor contextAccessor)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
            _contextAccessor = contextAccessor ?? throw new ArgumentNullException(nameof(contextAccessor));
            _settings = options.Value;
        }

        public async Task Invoke(HttpContext context)
        {
            context.TraceIdentifier = context.Request.Headers.ContainsKey(_settings.Header)
                ? context.Request.Headers[_settings.Header].ToString()
                : Guid.NewGuid().ToString();

            if (_settings.IncludeInResponse)
            {
                context.Response.OnStarting(() =>
                {
                    context.Response.Headers.TryAdd(_settings.Header, new[] {context.TraceIdentifier});

                    return Task.CompletedTask;
                });
            }

            _contextAccessor.Context = new ApplicationContext.ApplicationContext(context.TraceIdentifier, context.User.Id());

            await _next(context);
        }
    }
}