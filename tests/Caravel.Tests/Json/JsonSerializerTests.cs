using System.Text.Json;
using Caravel.Json;
using Xunit;

namespace Caravel.Tests.Json
{
    public class JsonSerializerTests
    {
        private record Dto(string Name);
        
        [Fact]
        public async Task Should_Serialize_Dto()
        {
            var model = new Dto("Caravel");

            var json = await model.SerializeAsync(new JsonSerializerOptions());
            
            Assert.Equal("{\"Name\":\"Caravel\"}", json);
        }
    }
}