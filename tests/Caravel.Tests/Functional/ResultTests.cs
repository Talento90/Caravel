using System.Collections.Generic;
using System.Linq;
using Caravel.Errors;
using Caravel.Functional;
using Xunit;

namespace Caravel.Tests.Functional;

public class ResultTests
{
    [Fact]
    public void Success_Should_Not_Contain_Errors()
    {
        var result = Result<string>.Success("Hello");

        Assert.Equal("Hello", result.Data);
        Assert.Empty(result.Errors);
        Assert.False(result.HasErrors);
    }
    
    [Fact]
    public void Non_Generic_Success_Should_Not_Contain_Errors()
    {
        var result = Result.Success();

        Assert.Empty(result.Errors);
        Assert.False(result.HasErrors);
    }

    [Fact]
    public void Failure_Should_Contain_Errors()
    {
        var result = Result<string>.Failure(new Error("test", "It's an error"));
        
        Assert.Null(result.Data);
        Assert.NotEmpty(result.Errors);
        Assert.Single(result.Errors);
        Assert.True(result.HasErrors);
        Assert.Equal("test", result.Errors.First().Code);
        Assert.Equal("It's an error", result.Errors.First().Message);

        result.AddError(new Error("test2", "It's another error"));
        Assert.Equal(2, result.Errors.Count());
        
        Assert.Equal("test2", result.Errors.Last().Code);
        Assert.Equal("It's another error", result.Errors.Last().Message);
    }
    
    [Fact]
    public void Non_Generic_Failure_Should_Contain_Errors()
    {
        var result = Result.Failure(new Error("test", "It's an error"));
        
        Assert.NotEmpty(result.Errors);
        Assert.Single(result.Errors);
        Assert.True(result.HasErrors);
        Assert.Equal("test", result.Errors.First().Code);
        Assert.Equal("It's an error", result.Errors.First().Message);

        result.AddError(new Error("test2", "It's another error"));
        Assert.Equal(2, result.Errors.Count());
        
        Assert.Equal("test2", result.Errors.Last().Code);
        Assert.Equal("It's another error", result.Errors.Last().Message);
    }

    [Fact]
    public void Failure_With_Multiple_Errors_Should_Contain_Errors()
    {
        var result = Result<string>.Failure(new List<Error>()
        {
            new ("test", "It's an error"),
            new ("test2", "It's another error"),
        });
        
        Assert.Null(result.Data);
        Assert.NotEmpty(result.Errors);
        Assert.Equal(2, result.Errors.Count());
        Assert.True(result.HasErrors);
        Assert.Equal("test", result.Errors.First().Code);
        Assert.Equal("It's an error", result.Errors.First().Message);
        
        Assert.Equal("test2", result.Errors.Last().Code);
        Assert.Equal("It's another error", result.Errors.Last().Message);
    }
    
    [Fact]
    public void Non_Generic_Failure_With_Multiple_Errors_Should_Contain_Errors()
    {
        var result = Result.Failure(new List<Error>()
        {
            new ("test", "It's an error"),
            new ("test2", "It's another error"),
        });
        
        Assert.NotEmpty(result.Errors);
        Assert.Equal(2, result.Errors.Count());
        Assert.True(result.HasErrors);
        Assert.Equal("test", result.Errors.First().Code);
        Assert.Equal("It's an error", result.Errors.First().Message);
        
        Assert.Equal("test2", result.Errors.Last().Code);
        Assert.Equal("It's another error", result.Errors.Last().Message);
    }
}