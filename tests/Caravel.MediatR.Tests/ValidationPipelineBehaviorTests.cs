using Caravel.Functional;
using Caravel.MediatR.Validation;
using FluentValidation;
using Xunit;

namespace Caravel.MediatR.Tests;

public class ValidationPipelineBehaviorTests
{
    private readonly List<IValidator<TestDataCommand>> _validators = [new TestDataCommand.TestDataCommandValidator()];
    
    [Fact]
    public async Task Should_Return_Generic_Error_Result_When_Returns_Result_And_Object_Is_Invalid()
    {
        var command = new TestDataCommand
        {
            Query = ""
        };

        var response = new TestDataResponse()
        {
            Data = ""
        };

        var behaviour = new ValidationPipelineBehavior<TestDataCommand, Result<TestDataResponse>>(_validators);

        var result = await behaviour.Handle(
            command,
            () => Task.FromResult(Result<TestDataResponse>.Success(response)),
            CancellationToken.None
        );

        Assert.False(result.IsSuccess);
        Assert.Equal(2, result.Error.ValidationErrors.Count);

        Assert.Equal("Id", result.Error.ValidationErrors[0].Identifier);
        Assert.Equal("'Id' must not be empty.", result.Error.ValidationErrors[0].Errors[0]);

        Assert.Equal("Query", result.Error.ValidationErrors[1].Identifier);
        Assert.Equal("'Query' must not be empty.", result.Error.ValidationErrors[1].Errors[0]);
        Assert.Equal("The specified condition was not met for 'Query'.", result.Error.ValidationErrors[1].Errors[1]);
    }
    
    [Fact]
    public async Task Should_Return_Non_Generic_Error_Result_When_Returns_Result_And_Object_Is_Invalid()
    {
        var command = new TestDataCommand
        {
            Query = ""
        };
        
        var behaviour = new ValidationPipelineBehavior<TestDataCommand, Result>(_validators);

        var result = await behaviour.Handle(
            command,
            () => Task.FromResult(Result.Success()),
            CancellationToken.None
        );

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

        var behaviour = new ValidationPipelineBehavior<TestDataCommand, Result<TestDataResponse>>(_validators);

        var result = await behaviour.Handle(
            command,
            () => Task.FromResult(Result<TestDataResponse>.Success(expectedResponse)),
            CancellationToken.None
        );

        Assert.Equal(expectedResponse, result.Data);
    }
    
    [Fact]
    public async Task Should_Throw_Validation_Exception_When_Response_Not_Result_And_Object_Is_Invalid()
    {
        var command = new TestDataCommand
        {
            Query = ""
        };

        var behaviour = new ValidationPipelineBehavior<TestDataCommand, TestDataResponse>(_validators);


        var ex = await Assert.ThrowsAsync<Exceptions.CaravelException>(() =>
            behaviour.Handle(command, () => Task.FromResult(new TestDataResponse {Data = string.Empty}),
                CancellationToken.None)
        );
        
        Assert.Equal(2, ex.Error.ValidationErrors.Count);

        Assert.Equal("Id", ex.Error.ValidationErrors[0].Identifier);
        Assert.Equal("'Id' must not be empty.", ex.Error.ValidationErrors[0].Errors[0]);
        
        Assert.Equal("Query", ex.Error.ValidationErrors[1].Identifier);
        Assert.Equal("'Query' must not be empty.", ex.Error.ValidationErrors[1].Errors[0]);
        Assert.Equal("The specified condition was not met for 'Query'.", ex.Error.ValidationErrors[1].Errors[1]);
    }
}