using System.Text.Json;
using Caravel.Json;
using Xunit;

namespace Caravel.Tests.Json
{
    public class JsonSerializerTests
    {
        private enum TestEnum
        {
            Undefined,
            Value1,
            Value2,
            ValueBig
        }
        
        private record Dto(string Name, TestEnum Value);
        
        [Fact]
        public async Task Should_Serialize_Camel_Case()
        {
            var model = new Dto("Caravel", TestEnum.Value1);
            var json = await model.SerializeAsync(JsonSerializerConfigurations.CamelCaseSerializer());
            
            Assert.Equal("{\"name\":\"Caravel\",\"value\":\"value1\"}", json);
        }
        
        [Fact]
        public void Should_Deserialize_Camel_Case()
        {
            var json = "{\"name\":\"Caravel\",\"value\":\"value1\"}";
            var model = JsonSerializer.Deserialize<Dto>(json, JsonSerializerConfigurations.CamelCaseSerializer());
            
            Assert.Equal("Caravel", model!.Name);
            Assert.Equal(TestEnum.Value1, model!.Value);
        }
        
        [Fact]
        public void Should_Deserialize_Unknown_Enums_Camel_Case()
        {
            var json = "{\"name\":\"Caravel\",\"value\":\"unknown\"}";
            var model = JsonSerializer.Deserialize<Dto>(json, JsonSerializerConfigurations.CamelCaseSerializer());
            
            Assert.Equal("Caravel", model!.Name);
            Assert.Equal(TestEnum.Undefined, model!.Value);
        }
        
        [Fact]
        public async Task Should_Serialize_Snake_Case()
        {
            var model = new Dto("Caravel", TestEnum.ValueBig);
            var json = await model.SerializeAsync(JsonSerializerConfigurations.SnakeCaseSerializer());
            
            Assert.Equal("{\"name\":\"Caravel\",\"value\":\"value_big\"}", json);
        }
        
        [Fact]
        public void Should_Deserialize_Snake_Case()
        {
            var json = "{\"name\":\"Caravel\",\"value\":\"value_big\"}";
            var model = JsonSerializer.Deserialize<Dto>(json, JsonSerializerConfigurations.SnakeCaseSerializer());
            
            Assert.Equal("Caravel", model!.Name);
            Assert.Equal(TestEnum.ValueBig, model!.Value);
        }
        
        [Fact]
        public void Should_Deserialize_Unknown_Enums_Snake_Case()
        {
            var json = "{\"name\":\"Caravel\",\"value\":\"unknown\"}";
            var model = JsonSerializer.Deserialize<Dto>(json, JsonSerializerConfigurations.SnakeCaseSerializer());
            
            Assert.Equal("Caravel", model!.Name);
            Assert.Equal(TestEnum.Undefined, model!.Value);
        }
    }
}