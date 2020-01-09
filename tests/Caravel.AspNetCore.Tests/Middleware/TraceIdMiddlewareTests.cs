using System.Collections.Generic;
using System.Threading.Tasks;
using Caravel.AspNetCore.Middleware;
using Caravel.Tests.Mocks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using Xunit;

namespace Caravel.AspNetCore.Tests.Middleware
{
    public class TraceIdMiddlewareTests
    {
        [Fact]
        public async Task Should_Generate_New_Trace_Id()
        {
            // Arrange
            var appContext = new AppContextMock();
            var middleware = new TraceIdMiddleware((innerHttpContext) => Task.CompletedTask, Options.Create(new TraceIdOptions()),  appContext);
            var httpContext = new DefaultHttpContext();
            //Act
            await middleware.Invoke(httpContext);

            //Assert
            Assert.Equal(appContext.Context.TraceId, httpContext.TraceIdentifier);
        }

        [Fact]
        public async Task Should_Reuse_Trace_Id()
        {
            // Arrange
            var appContext = new AppContextMock();
            var middleware = new TraceIdMiddleware((innerHttpContext) => Task.CompletedTask, Options.Create(new TraceIdOptions()), appContext);
            var context = new DefaultHttpContext();

            context.Request.Headers.Add(
                new KeyValuePair<string, StringValues>(TraceIdOptions.DefaultHeader, appContext.Context.TraceId)
            );

            //Act
            await middleware.Invoke(context);

            //Assert
            Assert.Equal(appContext.Context.TraceId, context.TraceIdentifier);
        }
    }
}