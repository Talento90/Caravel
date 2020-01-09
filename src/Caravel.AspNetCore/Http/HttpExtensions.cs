using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Http;
using JsonSerializerOptions = Caravel.AspNetCore.Http.JsonSerializerOptions;

namespace Caravel.AspNetCore.Http
{
    public static class HttpExtensions
    {
        public static string? Id(this ClaimsPrincipal user)
        {
            return user?.Claims?.FirstOrDefault(c => c.Type == "username")?.Value;
        }

        public static Task<HttpResponseMessage> PostJsonAsync<T>(this HttpClient httpClient, string requestUri, T body,
            CancellationToken ct)
        {
            var json = JsonSerializer.Serialize(body, JsonSerializerOptions.CamelCase());
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            return httpClient.PostAsync(requestUri, content, ct);
        }

        public static Task<HttpResponseMessage> PutJsonAsync<T>(this HttpClient httpClient, string requestUri, T body,
            CancellationToken ct)
        {
            var json = JsonSerializer.Serialize(body, JsonSerializerOptions.CamelCase());
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            return httpClient.PutAsync(requestUri, content, ct);
        }

        public static Task<HttpResponseMessage> PatchJsonAsync<T>(this HttpClient httpClient, string requestUri, T body,
            CancellationToken ct)
        {
            var json = JsonSerializer.Serialize(body, JsonSerializerOptions.CamelCase());
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            return httpClient.PatchAsync(requestUri, content, ct);
        }

        public static async Task<T> ReadAsJsonAsync<T>(this HttpContent content)
        {
            var json = await content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<T>(json, JsonSerializerOptions.CamelCase());
        }

        public static Task WriteJsonAsync<T>(this HttpResponse response, T obj)
        {
            response.ContentType = "application/json";
            
            var json = JsonSerializer.Serialize(obj, JsonSerializerOptions.CamelCase());

            return response.WriteAsync(json);
        }

        public static string BuildQueryString(this object obj)
        {
            var properties = obj.GetType()
                .GetProperties()
                .Where(p => p.GetValue(obj, null) != null)
                .Select(p => $"{p.Name}={HttpUtility.UrlEncode(p.GetValue(obj, null)?.ToString())}");

            return string.Join("&", properties.ToArray());
        }
    }
}