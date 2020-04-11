using System.Text.Json;
using System.Text.Json.Serialization;

namespace Caravel.Http
{
    public static class JsonSerializerOptions
    {
        /// <summary>
        /// Creates the default json configuration for Caravel applications.
        /// </summary>
        /// <returns></returns>
        public static System.Text.Json.JsonSerializerOptions CamelCase()
        {
            {
                var options = new System.Text.Json.JsonSerializerOptions()
                {
                    IgnoreNullValues = true,
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                };

                options.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
                options.Converters.Add(new JsonDateTimeConverter());

                return options;
            }
        }
    }
}