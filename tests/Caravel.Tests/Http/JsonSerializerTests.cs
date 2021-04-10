using System;
using Caravel.Http;
using Xunit;
using JsonSerializerOptions = Caravel.Http.JsonSerializerOptions;

namespace Caravel.Tests.Http
{
    public class JsonSerializerTests
    {
        private class Data
        {
            public DateTime Birthday { get; set; }
        }
        
        [Fact]
        public void Should_Serialize_DateTime_UTC()
        {
            var model = new Data
            {
                Birthday = DateTime.UtcNow
            };
            
            var json = JsonSerializer.Serialize(model, JsonSerializerOptions.CamelCase());

            var readModel = JsonSerializer.Deserialize<Data>(json, JsonSerializerOptions.CamelCase());

            Assert.Equal(model.Birthday, readModel!.Birthday);
        }
    }
}