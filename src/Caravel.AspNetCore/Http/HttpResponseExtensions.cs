using System.Text.Json;

namespace Caravel.AspNetCore.Http;

public static class HttpResponseExtensions
{
    public static async Task<T?> ReadAsJsonAsync<T>(this HttpResponse response, JsonSerializerOptions? options = null, CancellationToken ct = default)
    {
        return await JsonSerializer.DeserializeAsync<T>(response.Body, options, ct);
    }
}