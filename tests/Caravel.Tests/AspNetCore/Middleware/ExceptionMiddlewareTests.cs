using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Caravel.AspNetCore.Middleware;
using Caravel.Exceptions;
using Caravel.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Xunit;

namespace Caravel.Tests.AspNetCore.Middleware
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
                new LoggerFactory()
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
                new LoggerFactory()
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
            Assert.Equal(Errors.Error.Message, httpError.Detail);
            Assert.Equal(innerMessage, httpError.InnerMessage);
            Assert.Equal((int) HttpStatusCode.InternalServerError, httpError.Status);
        }

        [Fact]
        public async Task Should_Handle_Not_Found_Exception()
        {
            // Arrange
            var middleware = new ExceptionMiddleware(
                (innerHttpContext) => throw new NotFoundException(Errors.NotFound),
                new LoggerFactory()
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
            Assert.Equal(Errors.NotFound.Message, httpError.Detail);
            Assert.Equal((int) HttpStatusCode.NotFound, httpError.Status);
        }

        [Fact]
        public async Task Should_Handle_Bad_Validation_Exception()
        {
            // Arrange
            var middleware = new ExceptionMiddleware(
                (innerHttpContext) => throw new ValidationException(Errors.Validation),
                new LoggerFactory()
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
            Assert.Equal(Errors.Validation.Message, httpError.Detail);
            Assert.Equal((int) HttpStatusCode.BadRequest, httpError.Status);
        }

        [Fact]
        public async Task Should_Handle_Operation_Cancelled_Exception_Exception()
        {
            // Arrange
            var middleware = new ExceptionMiddleware(
                (innerHttpContext) => throw new OperationCancelledException(Errors.OperationWasCancelled),
                new LoggerFactory()
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
            Assert.Equal(Errors.OperationWasCancelled.Message, httpError.Detail);
            Assert.Equal((int) HttpStatusCode.Accepted, httpError.Status);
        }
        
        [Fact]
        public async Task Should_Handle_System_Operation_Cancelled_Exception_Exception()
        {
            // Arrange
            const string errorMessage = "Operation was cancelled";

            var middleware = new ExceptionMiddleware(
                (innerHttpContext) => throw new OperationCanceledException(errorMessage),
                new LoggerFactory()
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
            Assert.Equal(Errors.OperationWasCancelled.Message, httpError.Detail);
            Assert.Equal(errorMessage, httpError.InnerMessage);
            Assert.Equal((int) HttpStatusCode.Accepted, httpError.Status);
        }
        
        [Fact]
        public async Task Should_Handle_Unauthorized_Exception()
        {
            // Arrange
            var middleware = new ExceptionMiddleware(
                (innerHttpContext) => throw new UnauthorizedException(Errors.Unauthorized),
                new LoggerFactory()
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
            Assert.Equal(Errors.Unauthorized.Message, httpError.Detail);
            Assert.Equal((int) HttpStatusCode.Unauthorized, httpError.Status);
        }

        
        [Fact]
        public async Task Should_Handle_Permission_Exception()
        {
            // Arrange
            var middleware = new ExceptionMiddleware(
                (innerHttpContext) => throw new PermissionException(Errors.Permission),
                new LoggerFactory()
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
            Assert.Equal(Errors.Permission.Message, httpError.Detail);
            Assert.Equal((int) HttpStatusCode.Forbidden, httpError.Status);
        }
        
                
        [Fact]
        public async Task Should_Handle_Invalid_Operation_Exception()
        {
            // Arrange
            const string errorMessage = "User cannot perform this action";
            var middleware = new ExceptionMiddleware(
                (innerHttpContext) => throw new InvalidOperationException(errorMessage),
                new LoggerFactory()
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
            Assert.Equal(Errors.InvalidOperation.Message, httpError.Detail);
            Assert.Equal(errorMessage, httpError.InnerMessage);
            Assert.Equal((int) HttpStatusCode.BadRequest, httpError.Status);
        }
    }
}