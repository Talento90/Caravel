using Caravel.Errors;
using Caravel.Functional;
using Xunit;

namespace Caravel.Tests.Functional;

public class ResultTests
{
    [Fact]
    public void Should_Not_Contain_Errors()
    {
        var result = Result<string>.Success("Hello");

        Assert.Equal("Hello", result.Data);
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public void Should_Non_Generic_Not_Contain_Errors()
    {
        var result = Result.Success();

        Assert.True(result.IsSuccess);
    }

    [Fact]
    public void Should_Implicit_Operator_Convert_Error_To_Result()
    {
        Result<string> result = Error.Conflict("validation", "It's an error");

        Assert.False(result.IsSuccess);
        Assert.Equal("validation", result.Error.Code);
        Assert.Equal("It's an error", result.Error.Message);
    }

    [Fact]
    public void Should_Failure_Return_Error()
    {
        var result = Result<string>.Failure(Error.Validation("validation", "It's an error"));

        Assert.False(result.IsSuccess);
        Assert.Equal("validation", result.Error.Code);
        Assert.Equal("It's an error", result.Error.Message);
    }

    [Fact]
    public void Should_Non_Generic_Implicit_Operator_Convert_Error_To_Result()
    {
        Result result = Error.NotFound("not_found", "It's an error");

        Assert.False(result.IsSuccess);
        Assert.Equal("not_found", result.Error.Code);
        Assert.Equal("It's an error", result.Error.Message);
    }

    [Fact]
    public void Should_Map_Return_Success_Value()
    {
        var result = Result<string>.Success("Success Value")
            .Map((success) => success.Length,
                (error) => error.Message.Length
            );

        Assert.Equal("Success Value".Length, result);
    }

    [Fact]
    public void Should_Map_Return_Failure_Value()
    {
        var result = Result<string>.Failure(
                new Error("code_error", ErrorType.Validation, "Failure Value")
            )
            .Map((success) => success.Length,
                (error) => error.Message.Length
            );

        Assert.Equal("Failure Value".Length, result);
    }

    [Fact]
    public void Should_GetOrDefault_Return_DefaultValue()
    {
        var result = Result<string>.Failure(
            new Error("code", ErrorType.Validation, "Failure Value"));

        Assert.Equal("other_value", result.GetOrDefault("other_value"));
    }

    [Fact]
    public void Should_GetOrDefault_Return_Data()
    {
        var result = Result<string>.Success("some_value");

        Assert.Equal("some_value", result.GetOrDefault("other_value"));
    }
}