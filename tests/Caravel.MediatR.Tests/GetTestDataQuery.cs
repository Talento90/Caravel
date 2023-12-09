using FluentValidation;
using MediatR;

namespace Caravel.MediatR.Tests;

public record TestDataResponse
{
    public Guid? Id { get; set; }
    public required string Data { get; init; }
}

public record GetTestDataQuery : IRequest<TestDataResponse>
{
    public Guid Id { get; set; }
    public required string Query { get; init; }

    public class GetTestDataQueryValidator : AbstractValidator<GetTestDataQuery>
    {
        public GetTestDataQueryValidator()
        {
            RuleFor(t => t.Id).NotEmpty();
            RuleFor(t => t.Query)
                .NotEmpty()
                .Must(q => !string.IsNullOrEmpty(q) && q.StartsWith("q_"))
                .MinimumLength(3);
        }
    }
}