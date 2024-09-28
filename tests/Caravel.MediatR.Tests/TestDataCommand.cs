using Caravel.Functional;
using FluentValidation;
using MediatR;

namespace Caravel.MediatR.Tests;

public record TestDataCommand : IRequest<TestDataResponse>
{
    public Guid Id { get; set; }
    public required string Query { get; init; }

    public class TestDataCommandValidator : AbstractValidator<TestDataCommand>
    {
        public TestDataCommandValidator()
        {
            RuleFor(t => t.Id).NotEmpty();
            RuleFor(t => t.Query)
                .NotEmpty()
                .Must(q => !string.IsNullOrEmpty(q) && q.StartsWith("q_"))
                .MinimumLength(3);
        }
    }
}