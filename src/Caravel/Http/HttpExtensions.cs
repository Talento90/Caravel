using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Http;

namespace Caravel.Http
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

        public static void WriteJson<T>(this HttpResponse response, T obj, string? contentType = null)
        {
            response.ContentType = contentType ?? "application/json";

            using var writer = new Utf8JsonWriter(response.Body);
            
            JsonSerializer.Serialize(writer, obj, JsonSerializerOptions.CamelCase());
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