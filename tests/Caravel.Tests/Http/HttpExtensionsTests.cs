using Caravel.Http;
using Xunit;

namespace Caravel.Tests.Http
{
    public class HttpExtensionsTests
    {
        private class QueryModel
        {
            public int? Page { get; set; }
            public int? Count { get; set; }
            public bool? IsActive { get; set; }
            public string? Type { get; set; }
        }
        
        [Fact]
        public void Should_Build_Query_String()
        {
            var qsModel = new QueryModel
            {
                Page = 10,
                Count = 2,
                IsActive = true,
                Type = null
            };

            var qs = qsModel.BuildQueryString();

            Assert.Equal("Page=10&Count=2&IsActive=True", qs);
        }
    }
}