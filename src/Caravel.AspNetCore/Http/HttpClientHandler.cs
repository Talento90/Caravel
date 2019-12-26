using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Caravel.AppContext;
using Microsoft.AspNetCore.Http;

namespace Caravel.AspNetCore.Http
{
    public class HttpClientHandler : DelegatingHandler
    {
        private readonly IAppContext _appContext;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public HttpClientHandler(IAppContext appContext, IHttpContextAccessor httpContextAccessor)
        {
            _appContext = appContext;
            _httpContextAccessor = httpContextAccessor;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            request.Headers.Add("X-Correlation-Id", _appContext.Context.CorrelationId);

            if (_httpContextAccessor.HttpContext.Request.Headers.ContainsKey("User-Agent"))
            {
                var ua = _httpContextAccessor.HttpContext.Request.Headers["User-Agent"].ToString();
                request.Headers.TryAddWithoutValidation("User-Agent", ua);
            }

            return base.SendAsync(request, cancellationToken);
        }
    }
}