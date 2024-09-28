namespace Caravel.MediatR.Tests;

public record TestDataResponse
{
    public Guid? Id { get; set; }
    public required string Data { get; init; }
}