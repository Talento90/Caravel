using Caravel.Functional;
using Caravel.MediatR.Validation;
using FluentValidation;
using Xunit;

namespace Caravel.MediatR.Tests;

public class ResultRequestValidationBehaviorTests
{
    [Fact]
    public async Task Should_Return_Error_Result_When_Object_Is_Invalid()
    {
        var command = new TestDataCommand
        {
            Query = ""
        };

        var behaviour = new ResultValidationRequestBehavior<TestDataCommand, TestDataResponse>(
            new List<IValidator<TestDataCommand>>()
            {
                new TestDataCommand.TestDataCommandValidator()
            }
        );

        var result = await behaviour.Handle(command, () =>
            Task.FromResult(Result<TestDataResponse>.Success(new TestDataResponse()
            {
                Data = ""
            })), CancellationToken.None);
        
        Assert.False(result.IsSuccess);
        Assert.Equal(2, result.Error.ValidationErrors.Count);

        Assert.Equal("Id", result.Error.ValidationErrors[0].Identifier);
        Assert.Equal("'Id' must not be empty.", result.Error.ValidationErrors[0].Errors[0]);
        
        Assert.Equal("Query", result.Error.ValidationErrors[1].Identifier);
        Assert.Equal("'Query' must not be empty.", result.Error.ValidationErrors[1].Errors[0]);
        Assert.Equal("The specified condition was not met for 'Query'.", result.Error.ValidationErrors[1].Errors[1]);
    }

    [Fact]
    public async Task Should_Return_Success_Result_When_Object_Is_Valid()
    {
        var command = new TestDataCommand
        {
            Id = Guid.NewGuid(),
            Query = "q_random_query"
        };

        var expectedResponse = new TestDataResponse
        {
            Data = "success"
        };

        var behaviour = new ResultValidationRequestBehavior<TestDataCommand, TestDataResponse>(
            new List<IValidator<TestDataCommand>>()
            {
                new TestDataCommand.TestDataCommandValidator()
            }
        );
        
        var response = await behaviour.Handle(command, () => 
            Task.FromResult(Result<TestDataResponse>.Success(expectedResponse)),
            CancellationToken.None);

        Assert.Equal(expectedResponse, response);
    }
}