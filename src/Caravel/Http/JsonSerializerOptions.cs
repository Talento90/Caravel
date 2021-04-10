using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;


namespace Caravel.Http
{
    public static class JsonSerializerOptions
    {
        public static JsonSerializerSettings CamelCase()
        {
            return new JsonSerializerSettings()
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                NullValueHandling = NullValueHandling.Ignore,
                DateTimeZoneHandling = DateTimeZoneHandling.Utc,
                DateFormatHandling = DateFormatHandling.IsoDateFormat,
                Converters = new List<JsonConverter>()
                {
                    new StringEnumConverter(true)
                }
            };
        }
        
        public static JsonSerializerSettings SnakeCase()
        {
            return new JsonSerializerSettings()
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
                {
                    NamingStrategy = new SnakeCaseNamingStrategy()
                },
                NullValueHandling = NullValueHandling.Ignore,
                DateTimeZoneHandling = DateTimeZoneHandling.Utc,
                DateFormatHandling = DateFormatHandling.IsoDateFormat,
                Converters = new List<JsonConverter>()
                {
                    new StringEnumConverter(true)
                }
            };
        }
    }
}