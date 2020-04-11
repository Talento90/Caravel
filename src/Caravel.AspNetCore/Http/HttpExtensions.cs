using System;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Caravel.AspNetCore.Http
{
    public static class HttpExtensions
    {
        /// <summary>
        /// Get the current user id.
        /// </summary>
        /// <param name="user"></param>
        /// <returns>Return the current user id or null if does not exists.</returns>
        public static Guid? Id(this ClaimsPrincipal user)
        {
            var uid = user?.Claims?.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            if (Guid.TryParse(uid, out var result))
                return result;

            return null;
        }
        
        /// <summary>
        /// Write JSON to the HttpResponse using the <see cref="JsonSerializerOptions"/>.
        /// </summary>
        /// <param name="response"></param>
        /// <param name="obj"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static Task WriteJsonAsync<T>(this HttpResponse response, T obj)
        {
            response.ContentType = "application/json";

            var json = JsonSerializer.Serialize(obj, Caravel.Http.JsonSerializerOptions.CamelCase());

            return response.WriteAsync(json);
        }
    }
}