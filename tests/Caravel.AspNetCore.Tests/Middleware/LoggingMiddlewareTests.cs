using System;
using System.IO;
using System.Threading.Tasks;
using Caravel.AspNetCore.Middleware;
using Caravel.AspNetCore.Tests.Mocks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;

namespace Caravel.AspNetCore.Tests.Middleware
{
    public class LoggingMiddlewareTests
    {
        private static DefaultHttpContext CreateHttpContext()
        {
            var context = new DefaultHttpContext();

            context.Request.Host = new HostString("localhost:80");
            context.Request.Scheme = "https";

            return context;
        }

        [Fact]
        public async Task Should_Log_Request_Response()
        {
            // Arrange
            var loggerMock = new Mock<ILogger<LoggingMiddleware>>();

            var options = Options.Create(new LoggingSettings());
            var middleware = new LoggingMiddleware(
                (innerHttpContext) => Task.CompletedTask,
                loggerMock.Object,
                options,
                new AppContextAccessorMock()
            );

            var context = CreateHttpContext();
            context.Request.Path = "/api/v1/tests";
            context.Request.Body = new MemoryStream();
            context.Request.Scheme = "https";

            //Act
            await middleware.Invoke(context);

            //Assert
            loggerMock.Verify(l => l.BeginScope(It.IsAny<It.IsAnyType>()), Times.Exactly(1));

            loggerMock.Verify(l => l.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, _) => (v.ToString() ?? string.Empty).StartsWith("Request")),
                    It.IsAny<Exception>(),
                    (Func<It.IsAnyType, Exception, string>) It.IsAny<object>()),
                Times.Once
            );

            loggerMock.Verify(l => l.Log(
                    LogLevel.Information,
                    0,
                    It.Is<It.IsAnyType>((v, _) => (v.ToString() ?? string.Empty).StartsWith("Response")),
                    It.IsAny<Exception>(),
                    (Func<It.IsAnyType, Exception, string>) It.IsAny<object>()),
                Times.Once
            );
        }

        [Fact]
        public async Task Should_Log_Error_Response()
        {
            // Arrange
            var loggerMock = new Mock<ILogger<LoggingMiddleware>>();

            var options = Options.Create(new LoggingSettings());
            var middleware = new LoggingMiddleware(
                (innerHttpContext) => Task.CompletedTask,
                loggerMock.Object,
                options,
                new AppContextAccessorMock()
            );

            var context = CreateHttpContext();

            context.Request.Path = "/api/v1/tests";
            context.Request.Body = new MemoryStream();
            context.Request.Scheme = "https";
            context.Response.StatusCode = 500;

            //Act
            await middleware.Invoke(context);

            //Assert
            loggerMock.Verify(l => l.BeginScope(It.IsAny<It.IsAnyType>()), Times.Exactly(1));

            loggerMock.Verify(l => l.Log(
                    LogLevel.Information,
                    0,
                    It.Is<It.IsAnyType>((v, _) => (v.ToString() ?? string.Empty).StartsWith("Request")),
                    It.IsAny<Exception>(),
                    (Func<It.IsAnyType, Exception, string>) It.IsAny<object>()),
                Times.Once
            );

            loggerMock.Verify(l => l.Log(
                    LogLevel.Information,
                    0,
                    It.Is<It.IsAnyType>((v, _) => (v.ToString() ?? string.Empty).StartsWith("Response")),
                    It.IsAny<Exception>(),
                    (Func<It.IsAnyType, Exception, string>) It.IsAny<object>()),
                Times.Once
            );
        }

        [Fact]
        public async Task Should_Ignore_Paths()
        {
            // Arrange
            var loggerMock = new Mock<ILogger<LoggingMiddleware>>();

            var options = Options.Create(new LoggingSettings());
            var middleware = new LoggingMiddleware(
                (innerHttpContext) => Task.CompletedTask,
                loggerMock.Object,
                options,
                new AppContextAccessorMock()
            );

            var context = CreateHttpContext();

            context.Request.Path = "/health";

            //Act
            await middleware.Invoke(context);

            //Assert
            loggerMock.Verify(l => l.BeginScope(It.IsAny<It.IsAnyType>()), Times.Never());
        }

        [Fact]
        public async Task Should_Redact_Paths()
        {
            // Arrange
            var loggerMock = new Mock<ILogger<LoggingMiddleware>>();

            var options = Options.Create(new LoggingSettings()
            {
                PathsToRedact = new[] {"/api/v1/profile/password"}
            });

            var middleware = new LoggingMiddleware(
                (innerHttpContext) => Task.CompletedTask,
                loggerMock.Object,
                options,
                new AppContextAccessorMock()
            );

            var context = CreateHttpContext();

            context.Request.Path = "/api/v1/profile/password";

            //Act
            await middleware.Invoke(context);

            //Assert
            loggerMock.Verify(l => l.BeginScope(It.IsAny<It.IsAnyType>()), Times.Exactly(1));

            loggerMock.Verify(l => l.Log(
                    LogLevel.Information,
                    0,
                    It.Is<It.IsAnyType>((v, _) => (v.ToString() ?? string.Empty).Contains("[redacted]")),
                    It.IsAny<Exception>(),
                    (Func<It.IsAnyType, Exception, string>) It.IsAny<object>()),
                Times.Once
            );
        }

        [Fact]
        public async Task Should_Ignore_Authorization_Header()
        {
            // Arrange
            var loggerMock = new Mock<ILogger<LoggingMiddleware>>();

            var options = Options.Create(new LoggingSettings());
            var middleware = new LoggingMiddleware(
                (innerHttpContext) => Task.CompletedTask,
                loggerMock.Object,
                options,
                new AppContextAccessorMock()
            );

            var context = CreateHttpContext();

            context.Request.Path = "/api/v1/tests";
            context.Request.Body = new MemoryStream();
            context.Request.Headers.Add("Authorization", "JWT_TOKEN");
            context.Request.Headers.Add("User-Agent", "android");

            //Act
            await middleware.Invoke(context);

            //Assert
            loggerMock.Verify(l => l.Log(
                    LogLevel.Information,
                    0,
                    It.Is<It.IsAnyType>((v, _) => !(v.ToString() ?? string.Empty).Contains("Authorization")
                    ),
                    It.IsAny<Exception>(),
                    (Func<It.IsAnyType, Exception, string>) It.IsAny<object>()),
                Times.Exactly(2)
            );
        }

        [Fact]
        public async Task Should_Redact_Dynamic_Paths()
        {
            // Arrange
            var loggerMock = new Mock<ILogger<LoggingMiddleware>>();
            var options = Options.Create(new LoggingSettings
            {
                PathsToRedact = new[] {"/api/v1/devices/{*}/activate"}
            });

            var middleware = new LoggingMiddleware(
                (innerHttpContext) => Task.CompletedTask,
                loggerMock.Object,
                options,
                new AppContextAccessorMock()
            );

            var context = CreateHttpContext();

            context.Request.Path = "/api/v1/devices/guid-identifier/activate";

            //Act
            await middleware.Invoke(context);

            //Assert
            loggerMock.Verify(l => l.BeginScope(It.IsAny<It.IsAnyType>()), Times.Exactly(1));

            loggerMock.Verify(l => l.Log(
                    LogLevel.Information,
                    0,
                    It.Is<It.IsAnyType>((v, _) => (v.ToString() ?? string.Empty).Contains("[redacted]")),
                    It.IsAny<Exception>(),
                    (Func<It.IsAnyType, Exception, string>) It.IsAny<object>()),
                Times.Once
            );
        }

        [Fact]
        public async Task Should_Redact_Dynamic_Multiple_Paths()
        {
            // Arrange
            var loggerMock = new Mock<ILogger<LoggingMiddleware>>();
            var options = Options.Create(new LoggingSettings
            {
                PathsToRedact = new[] {"/api/v1/devices/{*}/activate/{*}"}
            });

            var middleware = new LoggingMiddleware(
                (innerHttpContext) => Task.CompletedTask,
                loggerMock.Object,
                options,
                new AppContextAccessorMock()
            );

            var context = CreateHttpContext();

            context.Request.Path = "/api/v1/devices/guid-identifier/activate/{another-guid}";

            //Act
            await middleware.Invoke(context);

            //Assert
            loggerMock.Verify(l => l.BeginScope(It.IsAny<It.IsAnyType>()), Times.Exactly(1));

            loggerMock.Verify(l => l.Log(
                    LogLevel.Information,
                    0,
                    It.Is<It.IsAnyType>((v, _) => (v.ToString() ?? string.Empty).Contains("[redacted]")),
                    It.IsAny<Exception>(),
                    (Func<It.IsAnyType, Exception, string>) It.IsAny<object>()),
                Times.Once
            );
        }
    }
}