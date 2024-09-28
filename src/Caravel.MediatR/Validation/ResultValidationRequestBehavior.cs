using Caravel.Errors;
using Caravel.Functional;
using FluentValidation;
using MediatR;

namespace Caravel.MediatR.Validation;

public class ResultValidationRequestBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, Result<TResponse>>
    where TRequest : IRequest<TResponse>
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ResultValidationRequestBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<Result<TResponse>> Handle(TRequest request, RequestHandlerDelegate<Result<TResponse>> next,
        CancellationToken cancellationToken)
    {
        var context = new ValidationContext<TRequest>(request);

        var errors = _validators
            .Select(v => v.Validate(context))
            .SelectMany(result => result.Errors)
            .Where(e => e != null)
            .GroupBy(k => k.PropertyName, v => v)
            .ToDictionary(k => k.Key, v => v.Select(e => e.ErrorMessage).ToArray())
            .Select(err => new ValidationError(err.Key, err.Value))
            .ToList();

        if (errors.Count != 0)
        {
            return Error.Validation(ErrorCodes.ValidationError, "Invalid fields", errors);
        }

        return await next();
    }
}