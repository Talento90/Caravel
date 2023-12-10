using System.Net;
using Caravel.AspNetCore.Http;
using Caravel.AspNetCore.Middleware;
using Caravel.Errors;
using Caravel.Exceptions;
using Newtonsoft.Json;
using Xunit;

namespace Caravel.AspNetCore.Tests.Middleware;

public class ExceptionMiddlewareTests
{
    [Fact]
    public async Task Should_Handle_Exception_Success()
    {
        // Arrange
        var exceptionHandler = new GlobalExceptionHandler(new LoggerFactory().CreateLogger<GlobalExceptionHandler>());
        var context = new DefaultHttpContext
        {
            Response =
            {
                Body = new MemoryStream()
            }
        };

        var result =
            await exceptionHandler.TryHandleAsync(context, new Exception("Exception"), CancellationToken.None);

        context.Response.Body.Seek(0, SeekOrigin.Begin);
        var reader = new StreamReader(context.Response.Body);
        var streamText = await reader.ReadToEndAsync();
        var httpError = JsonConvert.DeserializeObject<HttpError>(streamText);

        //Assert
        Assert.Equal(StatusCodes.Status500InternalServerError, context.Response.StatusCode);
        Assert.Equal("Server Error", httpError?.Title);
        Assert.True(result);
    }
    
    [Fact]
    public async Task Should_Handle_Caravel_Exception_Success()
    {
        // Arrange
        var exceptionHandler = new GlobalExceptionHandler(new LoggerFactory().CreateLogger<GlobalExceptionHandler>());
        var context = new DefaultHttpContext
        {
            Response =
            {
                Body = new MemoryStream()
            }
        };

        var result = await exceptionHandler.TryHandleAsync(
            context,
            new CaravelException(Error.Internal("internal", "Some Error Occured")),
            CancellationToken.None);

        context.Response.Body.Seek(0, SeekOrigin.Begin);
        var reader = new StreamReader(context.Response.Body);
        var streamText = await reader.ReadToEndAsync();
        var httpError = JsonConvert.DeserializeObject<HttpError>(streamText);

        //Assert
        Assert.Equal(StatusCodes.Status500InternalServerError, context.Response.StatusCode);
        Assert.Equal("internal", httpError?.Code);
        Assert.Equal("Some Error Occured", httpError?.Title);
        Assert.True(result);
    }

    [Fact]
    public async Task Should_Handle_Validation_Exception_Success()
    {
        // Arrange
        var exceptionHandler = new GlobalExceptionHandler(new LoggerFactory().CreateLogger<GlobalExceptionHandler>());
        var context = new DefaultHttpContext
        {
            Response =
            {
                Body = new MemoryStream()
            }
        };

        var errorFields = new List<ValidationError>()
        {
            new ("name", new [] {"Name cannot be empty", "Name must be longer than 5 characters"}),
            new ("name", new [] {"Name duplicated"}),
            new ("age", new [] {"You need be older than 18"})
        };

        var result = await exceptionHandler.TryHandleAsync(
            context,
            new CaravelException(Error.Validation("invalid_fields", "Error processing request", errorFields)),
            CancellationToken.None);

        context.Response.Body.Seek(0, SeekOrigin.Begin);
        var reader = new StreamReader(context.Response.Body);
        var streamText = await reader.ReadToEndAsync();
        var httpError = JsonConvert.DeserializeObject<HttpError>(streamText);

        //Assert
        Assert.Equal(StatusCodes.Status400BadRequest, context.Response.StatusCode);
        Assert.Equal("invalid_fields", httpError?.Code);
        Assert.Equal(3, httpError?.Errors?["name"].Count());
        Assert.Equal("Name cannot be empty", httpError?.Errors?["name"].First());
        Assert.Equal("You need be older than 18", httpError?.Errors?["age"].First());

        Assert.True(result);
    }
}