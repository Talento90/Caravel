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
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            PropertyNameCaseInsensitive = true,
            Converters = { new JsonStringUnknownEnumConverter(JsonNamingPolicy.CamelCase) },
        };
    }

    public static JsonSerializerOptions SnakeCaseSerializer()
    {
        return new JsonSerializerOptions(JsonSerializerDefaults.Web)
        {
            PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            PropertyNameCaseInsensitive = true,
            Converters = { new JsonStringUnknownEnumConverter(JsonNamingPolicy.SnakeCaseLower) },
        };
    }
}