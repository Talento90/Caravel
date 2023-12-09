using Caravel.Errors;
using Caravel.Functional;
using Caravel.Result;
using Xunit;

namespace Caravel.Tests.Functional;

public class ResultTests
{
    [Fact]
    public void Should_Not_Contain_Errors()
    {
        var result = Result<string, Error<TestErrorCodes>>.Success("Hello");

        Assert.Equal("Hello", result.Data);
        Assert.False(result.HasError);
    }

    [Fact]
    public void Should_Non_Generic_Not_Contain_Errors()
    {
        var result = Result<TestErrorCodes>.Success();

        Assert.False(result.HasError);
    }

    [Fact]
    public void Should_Implicit_Operator_Convert_Error_To_Result()
    {
        Result<string, TestErrorCodes> result =
            new Error<TestErrorCodes>(TestErrorCodes.InvalidOperation, ErrorType.Validation, "It's an error");

        Assert.True(result.HasError);
        Assert.Equal(TestErrorCodes.InvalidOperation, result.Error.Code);
        Assert.Equal("It's an error", result.Error.Message);
    }

    [Fact]
    public void Should_Failure_Return_Error()
    {
        var result = Result<string, TestErrorCodes>.Failure(
            new Error<TestErrorCodes>(TestErrorCodes.InvalidOperation, ErrorType.Validation, "It's an error")
        );

        Assert.True(result.HasError);
        Assert.Equal(TestErrorCodes.InvalidOperation, result.Error.Code);
        Assert.Equal("It's an error", result.Error.Message);
    }

    [Fact]
    public void Should_Non_Generic_Implicit_Operator_Convert_Error_To_Result()
    {
        Result<TestErrorCodes> result =
            new Error<TestErrorCodes>(TestErrorCodes.InvalidOperation, ErrorType.Validation, "It's an error");

        Assert.True(result.HasError);
        Assert.Equal(TestErrorCodes.InvalidOperation, result.Error.Code);
        Assert.Equal("It's an error", result.Error.Message);
    }

    [Fact]
    public void Should_Map_Return_Success_Value()
    {
        var result = Result<string, Error<TestErrorCodes>>.Success("Success Value")
            .Map((success) => success.Length,
                (error) => error.Message.Length
            );

        Assert.Equal("Success Value".Length, result);
    }

    [Fact]
    public void Should_Map_Return_Failure_Value()
    {
        var result = Result<string, TestErrorCodes>.Failure(
                new Error<TestErrorCodes>(TestErrorCodes.InvalidOperation, ErrorType.Validation, "Failure Value"))
            .Map((success) => success.Length,
                (error) => error.Message.Length
            );

        Assert.Equal("Failure Value".Length, result);
    }

    [Fact]
    public void Should_GetOrDefault_Return_DefaultValue()
    {
        var result = Result<string, TestErrorCodes>.Failure(
            new Error<TestErrorCodes>(TestErrorCodes.InvalidOperation, ErrorType.Validation, "Failure Value"));

        Assert.Equal("other_value", result.GetOrDefault("other_value"));
    }

    [Fact]
    public void Should_GetOrDefault_Return_Data()
    {
        var result = Result<string, Error<TestErrorCodes>>.Success("some_value");

        Assert.Equal("some_value", result.GetOrDefault("other_value"));
    }
}