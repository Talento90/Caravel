using FluentValidation;
using Xunit;

namespace Caravel.MediatR.Tests;

public class RequestValidationBehaviorTests
{
    [Fact]
    public async Task Should_Throw_Validation_Exception_When_Object_Is_Invalid()
    {
        var query = new GetTestDataQuery
        {
            Query = string.Empty
        };

        var behaviour = new RequestValidationBehavior<GetTestDataQuery, TestDataResponse>(
            new List<IValidator<GetTestDataQuery>>()
            {
                new GetTestDataQuery.GetTestDataQueryValidator()
            }
        );

        var ex = await Assert.ThrowsAsync<Exceptions.CaravelException>(() =>
            behaviour.Handle(query, () => Task.FromResult(new TestDataResponse {Data = string.Empty}),
                CancellationToken.None)
        );
        
        Assert.Equal(2, ex.Error.ValidationErrors.Count);

        // Assert.Equal("'Id' must not be empty.", ex.Error.ValidationErrors["Id"][0]);
        // Assert.Equal("'Query' must not be empty.", ex.Error.ValidationErrors["Query"][0]);
        // Assert.Equal("The specified condition was not met for 'Query'.", ex.Error.ValidationErrors["Query"][1]);
    }

    [Fact]
    public async Task Should_Not_Throw_Validation_Exception_When_Object_Is_Valid()
    {
        var query = new GetTestDataQuery
        {
            Id = Guid.NewGuid(),
            Query = "q_random_query"
        };

        var expectedResponse = new TestDataResponse
        {
            Data = string.Empty
        };

        var behaviour = new RequestValidationBehavior<GetTestDataQuery, TestDataResponse>(
            new List<IValidator<GetTestDataQuery>>()
            {
                new GetTestDataQuery.GetTestDataQueryValidator()
            }
        );

        var response =
            await behaviour.Handle(query, () => Task.FromResult(expectedResponse), CancellationToken.None);

        Assert.Equal(expectedResponse, response);
    }
}