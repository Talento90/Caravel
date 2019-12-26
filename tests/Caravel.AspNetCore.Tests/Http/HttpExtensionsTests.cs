using Caravel.AspNetCore.Http;
using Xunit;

namespace Caravel.AspNetCore.Tests.Http
{
    public class HttpExtensionsTests
    {
        [Fact]
        public void Should_Build_Query_String()
        {
            var qsModel = new
            {
                Page = 10,
                Count = 2,
                IsActive = true,
                Type = (string?) null
            };

            var qs = qsModel.BuildQueryString();

            Assert.Equal("Page=10&Count=2&IsActive=True", qs);
        }
    }
}