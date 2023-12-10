using Caravel.Errors;
using Caravel.Exceptions;
using FluentValidation;
using MediatR;
namespace Caravel.MediatR;

public class RequestValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public RequestValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
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
            throw new CaravelException(Error.Validation("invalid_fields", "Invalid fields", errors));
        }

        return next();
    }
}