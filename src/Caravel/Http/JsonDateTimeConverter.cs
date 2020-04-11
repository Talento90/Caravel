using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Caravel.Http
{
    /// <summary>
    /// JsonDateTimeConverter is a JsonConverter to convert DateTime in the correct format and to UTC.
    /// </summary>
    public class JsonDateTimeConverter : JsonConverter<DateTime>
    {
        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, System.Text.Json.JsonSerializerOptions options)
        {
            return DateTime.Parse(reader.GetString()).ToUniversalTime();
        }

        public override void Write(Utf8JsonWriter writer, DateTime value, System.Text.Json.JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToUniversalTime().ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss.ffffffK"));
        }
    }
}