using System;
using Newtonsoft.Json;

namespace Caravel.Http
{
    public static class JsonSerializer
    {
        public static string Serialize<T>(T obj)
        {
            return JsonConvert.SerializeObject(obj, JsonSerializerOptions.CamelCase());
        }
        
        public static string Serialize<T>(T obj, JsonSerializerSettings settings)
        {
            return JsonConvert.SerializeObject(obj, settings);
        }

        public static T Deserialize<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json, JsonSerializerOptions.CamelCase())
                   ?? throw new JsonException();
        }
        
        public static T Deserialize<T>(string json, JsonSerializerSettings settings)
        {
            return JsonConvert.DeserializeObject<T>(json, settings)
                   ?? throw new JsonException();
        }

        public static object Deserialize(string json, Type type)
        {
            return JsonConvert.DeserializeObject(json, type, JsonSerializerOptions.CamelCase())
                   ?? throw new JsonException();;
        }

        public static bool TryDeserialize(string json, Type type, out object obj)
        {
            obj = null!;
            try
            {
                obj = Deserialize(json, type);
                return true;
            }
            catch (JsonException)
            {
                return false;
            }
        }
    }
}