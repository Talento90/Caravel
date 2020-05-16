using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Caravel.AspNetCore.Http;
using Caravel.AspNetCore.Middleware;
using Caravel.Errors;
using Caravel.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Xunit;

namespace Caravel.AspNetCore.Tests.Middleware
{
    public class ExceptionMiddlewareTests
    {
        [Fact]
        public async Task Should_Handle_Success()
        {
            // Arrange
            var middleware = new ExceptionMiddleware(
                (innerHttpContext) =>
                {
                    var dummyResponse = new DummyResponse("dummy");
                    var json = JsonConvert.SerializeObject(dummyResponse);
                    var bytes = Encoding.ASCII.GetBytes(json);

                    innerHttpContext.Response.Body = new MemoryStream(bytes);

                    return Task.CompletedTask;
                },
                new LoggerFactory().CreateLogger<ExceptionMiddleware>()
            );

            var context = new DefaultHttpContext();
            context.Response.Body = new MemoryStream();

            //Act
            await middleware.Invoke(context);

            context.Response.Body.Seek(0, SeekOrigin.Begin);
            var reader = new StreamReader(context.Response.Body);
            var streamText = reader.ReadToEnd();
            var dummy = JsonConvert.DeserializeObject<DummyResponse>(streamText);

            //Assert
            Assert.Equal((int) HttpStatusCode.OK, context.Response.StatusCode);
            Assert.Equal("dummy", dummy.Name);
        }

        [Fact]
        public async Task Should_Handle_Unknown_Exception()
        {
            // Arrange
            const string innerMessage = "Unknown Exception Message";
            var middleware = new ExceptionMiddleware(
                (innerHttpContext) => throw new Exception(innerMessage),
                new LoggerFactory().CreateLogger<ExceptionMiddleware>()
            );

            var context = new DefaultHttpContext();
            context.Response.Body = new MemoryStream();

            //Act
            await middleware.Invoke(context);

            context.Response.Body.Seek(0, SeekOrigin.Begin);
            var reader = new StreamReader(context.Response.Body);
            var streamText = reader.ReadToEnd();
            var httpError = JsonConvert.DeserializeObject<HttpError>(streamText);

            //Assert
            Assert.NotNull(httpError);
            Assert.Equal("Internal error server.", httpError.Title);
            Assert.Equal((int) HttpStatusCode.InternalServerError, httpError.Status);
        }

        [Fact]
        public async Task Should_Handle_Not_Found_Exception()
        {
            // Arrange
            var middleware = new ExceptionMiddleware(
                (innerHttpContext) => throw new NotFoundException(new Error("user_not_found", "User not found")),
                new LoggerFactory().CreateLogger<ExceptionMiddleware>()
            );

            var context = new DefaultHttpContext();
            context.Response.Body = new MemoryStream();

            //Act
            await middleware.Invoke(context);

            context.Response.Body.Seek(0, SeekOrigin.Begin);
            var reader = new StreamReader(context.Response.Body);
            var streamText = await reader.ReadToEndAsync();
            var httpError = JsonConvert.DeserializeObject<HttpError>(streamText);

            //Assert
            Assert.NotNull(httpError);
            Assert.Equal("User not found", httpError.Title);
            Assert.Equal((int) HttpStatusCode.NotFound, httpError.Status);
        }

        [Fact]
        public async Task Should_Handle_Bad_Validation_Exception()
        {
            // Arrange
            var middleware = new ExceptionMiddleware(
                (innerHttpContext) => throw new ValidationException(new Error("validation", "invalid field")),
                new LoggerFactory().CreateLogger<ExceptionMiddleware>()
            );

            var context = new DefaultHttpContext();
            context.Response.Body = new MemoryStream();

            //Act
            await middleware.Invoke(context);

            context.Response.Body.Seek(0, SeekOrigin.Begin);
            var reader = new StreamReader(context.Response.Body);
            var streamText = reader.ReadToEnd();
            var httpError = JsonConvert.DeserializeObject<HttpError>(streamText);

            //Assert
            Assert.NotNull(httpError);
            Assert.Equal("validation", httpError.Code);
            Assert.Equal("invalid field", httpError.Title);
            Assert.Equal((int) HttpStatusCode.BadRequest, httpError.Status);
        }

        [Fact]
        public async Task Should_Handle_Unauthorized_Exception()
        {
            // Arrange
            var middleware = new ExceptionMiddleware(
                (innerHttpContext) => throw new UnauthorizedException(new Error("unauthorized", "not logged in")),
                new LoggerFactory().CreateLogger<ExceptionMiddleware>()
            );

            var context = new DefaultHttpContext();
            context.Response.Body = new MemoryStream();

            //Act
            await middleware.Invoke(context);

            context.Response.Body.Seek(0, SeekOrigin.Begin);
            var reader = new StreamReader(context.Response.Body);
            var streamText = reader.ReadToEnd();
            var httpError = JsonConvert.DeserializeObject<HttpError>(streamText);

            //Assert
            Assert.NotNull(httpError);
            Assert.Equal("unauthorized", httpError.Code);
            Assert.Equal("not logged in", httpError.Title);
            Assert.Equal((int) HttpStatusCode.Unauthorized, httpError.Status);
        }


        [Fact]
        public async Task Should_Handle_Permission_Exception()
        {
            // Arrange
            var middleware = new ExceptionMiddleware(
                (innerHttpContext) => throw new PermissionException(new Error("permission", "no permissions")),
                new LoggerFactory().CreateLogger<ExceptionMiddleware>()
            );

            var context = new DefaultHttpContext();
            context.Response.Body = new MemoryStream();

            //Act
            await middleware.Invoke(context);

            context.Response.Body.Seek(0, SeekOrigin.Begin);
            var reader = new StreamReader(context.Response.Body);
            var streamText = reader.ReadToEnd();
            var httpError = JsonConvert.DeserializeObject<HttpError>(streamText);

            //Assert
            Assert.NotNull(httpError);
            Assert.Equal("no permissions", httpError.Title);
            Assert.Equal((int) HttpStatusCode.Forbidden, httpError.Status);
        }
    }
}