using System.Collections.Generic;
using System.Threading.Tasks;
using Caravel.AspNetCore.Middleware;
using Caravel.Tests.Mocks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using Xunit;

namespace Caravel.Tests.AspNetCore.Middleware
{
    public class CorrelationIdMiddlewareTests
    {
        [Fact]
        public async Task Should_Generate_New_Correlation_Id()
        {
            // Arrange
            var appContext = new AppContextMock();
            var middleware = new CorrelationIdMiddleware((innerHttpContext) => Task.CompletedTask, appContext);
            var httpContext = new DefaultHttpContext();
            //Act
            await middleware.Invoke(httpContext);

            //Assert
            Assert.Equal(appContext.Context.CorrelationId, httpContext.TraceIdentifier);
        }

        [Fact]
        public async Task Should_Reuse_Correlation_Id()
        {
            // Arrange
            var appContext = new AppContextMock();
            var middleware = new CorrelationIdMiddleware((innerHttpContext) => Task.CompletedTask, appContext);
            var context = new DefaultHttpContext();

            context.Request.Headers.Add(
                new KeyValuePair<string, StringValues>(CorrelationIdMiddleware.CorrelationIdHeader, appContext.Context.CorrelationId)
            );

            //Act
            await middleware.Invoke(context);

            //Assert
            Assert.Equal(appContext.Context.CorrelationId, context.TraceIdentifier);
        }
    }
}