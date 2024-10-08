using System.Text;
using System.Text.Json;

namespace Caravel.Json;

public static class JsonSerializerExtensions
{
    public static async Task<string> SerializeAsync<T>(this T obj, JsonSerializerOptions serializerOptions,
        CancellationToken cancellationToken = default)
        where T : class
    {
        await using var memoryStream = new MemoryStream();
        await JsonSerializer.SerializeAsync(memoryStream, obj, serializerOptions, cancellationToken);
        return Encoding.UTF8.GetString(memoryStream.ToArray());
    }

    public static string Serialize<T>(this T obj, JsonSerializerOptions serializerOptions)
        where T : class
    {
        using var memoryStream = new MemoryStream();
        JsonSerializer.Serialize(memoryStream, obj, serializerOptions);
        return Encoding.UTF8.GetString(memoryStream.ToArray());
    }
}