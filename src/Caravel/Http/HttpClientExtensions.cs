using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace Caravel.Http
{
    public static class HttpExtensions
    {
        /// <summary>
        /// Sends a POST request to the specific requestUri and serializes the body as JSON using the <see cref="JsonSerializerOptions"/>. 
        /// </summary>
        /// <param name="httpClient">The HttpClient.</param>
        /// <param name="requestUri">Request Uri.</param>
        /// <param name="body">Body to send as a JSON.</param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static Task<HttpResponseMessage> PostJsonAsync<T>(this HttpClient httpClient, string requestUri, T body)
        {
            return PostJsonAsync(httpClient, requestUri, body, CancellationToken.None);
        }

        /// <summary>
        /// Sends a POST request to the specific requestUri and serializes the body as JSON using the <see cref="JsonSerializerOptions"/>. 
        /// </summary>
        /// <param name="httpClient">The HttpClient.</param>
        /// <param name="requestUri">Request Uri.</param>
        /// <param name="body">Body to send as a JSON.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static Task<HttpResponseMessage> PostJsonAsync<T>(this HttpClient httpClient, string requestUri, T body, CancellationToken ct)
        {
            var json = JsonSerializer.Serialize(body, JsonSerializerOptions.CamelCase());
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            return httpClient.PostAsync(requestUri, content, ct);
        }

        /// <summary>
        /// Sends a PUT request to the specific requestUri and serializes the body as JSON using the <see cref="JsonSerializerOptions"/>. 
        /// </summary>
        /// <param name="httpClient">The HttpClient.</param>
        /// <param name="requestUri">Request Uri.</param>
        /// <param name="body">Body to send as a JSON.</param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static Task<HttpResponseMessage> PutJsonAsync<T>(this HttpClient httpClient, string requestUri, T body) 
        {
            return PutJsonAsync(httpClient, requestUri, body, CancellationToken.None);
        }
        
        /// <summary>
        /// Sends a PUT request to the specific requestUri and serializes the body as JSON using the <see cref="JsonSerializerOptions"/>. 
        /// </summary>
        /// <param name="httpClient">The HttpClient.</param>
        /// <param name="requestUri">Request Uri.</param>
        /// <param name="body">Body to send as a JSON.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static Task<HttpResponseMessage> PutJsonAsync<T>(this HttpClient httpClient, string requestUri, T body, CancellationToken ct)
        {
            var json = JsonSerializer.Serialize(body, JsonSerializerOptions.CamelCase());
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            return httpClient.PutAsync(requestUri, content, ct);
        }

        /// <summary>
        /// Sends a PATCH request to the specific requestUri and serializes the body as JSON using the <see cref="JsonSerializerOptions"/>. 
        /// </summary>
        /// <param name="httpClient">The HttpClient.</param>
        /// <param name="requestUri">Request Uri.</param>
        /// <param name="body">Body to send as a JSON.</param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static Task<HttpResponseMessage> PatchJsonAsync<T>(this HttpClient httpClient, string requestUri, T body)
        {
            return PatchJsonAsync(httpClient, requestUri, body, CancellationToken.None);
        }
        
        /// <summary>
        /// Sends a PATCH request to the specific requestUri and serializes the body as JSON using the <see cref="JsonSerializerOptions"/>. 
        /// </summary>
        /// <param name="httpClient">The HttpClient.</param>
        /// <param name="requestUri">Request Uri.</param>
        /// <param name="body">Body to send as a JSON.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static Task<HttpResponseMessage> PatchJsonAsync<T>(this HttpClient httpClient, string requestUri, T body, CancellationToken ct)
        {
            var json = JsonSerializer.Serialize(body, JsonSerializerOptions.CamelCase());
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            return httpClient.PatchAsync(requestUri, content, ct);
        }

        /// <summary>
        /// Read the HttpContent body as JSON using the <see cref="JsonSerializerOptions"/>.
        /// </summary>
        /// <param name="content">Content to read.</param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static async Task<T> ReadAsJsonAsync<T>(this HttpContent content)
        {
            var json = await content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<T>(json, JsonSerializerOptions.CamelCase()) ?? throw new InvalidOperationException();
        }

        /// <summary>
        /// Generate a query string using the public properties of the object.
        /// </summary>
        /// <param name="obj">Object to extract the query string.</param>
        /// <returns>The generated query string.</returns>
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