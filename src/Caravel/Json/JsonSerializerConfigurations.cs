using System.Text.Json;
using System.Text.Json.Serialization;

namespace Caravel.Json;
public static class JsonSerializerConfigurations
{
    public static JsonSerializerOptions CamelCaseSerializer()
    {
        return new JsonSerializerOptions(JsonSerializerDefaults.Web)
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            Converters = { new JsonStringEnumConverter() },
        };
    }

    public static JsonSerializerOptions SnakeCaseSerializer()
    {
        return new JsonSerializerOptions(JsonSerializerDefaults.Web)
        {
            PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
            Converters = { new JsonStringEnumConverter() },
        };
    }
}