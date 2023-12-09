using System.Net;
using Caravel.AspNetCore.Middleware;
using Caravel.Exceptions;
using Microsoft.AspNetCore.Mvc;
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
            await exceptionHandler.TryHandleAsync(context, new CaravelException("Exception"), CancellationToken.None);

        context.Response.Body.Seek(0, SeekOrigin.Begin);
        var reader = new StreamReader(context.Response.Body);
        var streamText = await reader.ReadToEndAsync();
        var problemDetails = JsonConvert.DeserializeObject<ProblemDetails>(streamText);

        //Assert
        Assert.Equal(StatusCodes.Status500InternalServerError, context.Response.StatusCode);
        Assert.Equal("Server Error", problemDetails?.Title);
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

        var errorFields = new Dictionary<string, string[]>()
        {
            {"name", new [] {"Name cannot be empty"}}
        };

        var result = await exceptionHandler.TryHandleAsync(
            context,
            new ValidationException("Exception", errorFields),
            CancellationToken.None);

        context.Response.Body.Seek(0, SeekOrigin.Begin);
        var reader = new StreamReader(context.Response.Body);
        var streamText = await reader.ReadToEndAsync();
        var problemDetails = JsonConvert.DeserializeObject<ProblemDetails>(streamText);

        //Assert
        Assert.Equal(StatusCodes.Status400BadRequest, context.Response.StatusCode);
        Assert.Equal("Invalid Request", problemDetails?.Title);
        Assert.Equal("Invalid Request", problemDetails?.Extensions["errors"]);
        Assert.True(result);
    }
}