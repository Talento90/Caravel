using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Caravel.AspNetCore.Http
{
    /// <summary>
    /// HttpClient Handler to proxy the authorization header.
    /// </summary>
    public class AuthorizationProxyHttpClientHandler : DelegatingHandler
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthorizationProxyHttpClientHandler(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            if (_httpContextAccessor.HttpContext.Request.Headers.ContainsKey("Authorization"))
            {
                request.Headers.Add("Authorization",
                    _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString());
            }

            return base.SendAsync(request, cancellationToken);
        }
    }
}