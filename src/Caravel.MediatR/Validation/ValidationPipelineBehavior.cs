using System.Reflection;
using Caravel.Errors;
using Caravel.Exceptions;
using Caravel.Functional;
using FluentValidation;
using MediatR;

namespace Caravel.MediatR.Validation;

public sealed class ValidationPipelineBehavior<TRequest, TResponse>(
    IEnumerable<IValidator<TRequest>> validators)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : class
{

    private const string ErrorMessage = "Error validating payload.";
    
    public async Task<TResponse> Handle(
        TRequest request, 
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var context = new ValidationContext<TRequest>(request);

        var errors = validators
            .Select(v => v.Validate(context))
            .SelectMany(result => result.Errors)
            .Where(e => e != null)
            .GroupBy(k => k.PropertyName, v => v)
            .ToDictionary(k => k.Key, v => v.Select(e => e.ErrorMessage).ToArray())
            .Select(err => new ValidationError(err.Key, err.Value))
            .ToList();

        if (errors.Count == 0)
        {
            return await next();
        }
        
        if (typeof(TResponse).IsGenericType && typeof(TResponse).GetGenericTypeDefinition() == typeof(Result<>))
        {
            Type resultType = typeof(TResponse).GetGenericArguments()[0];

            MethodInfo? failureMethod = typeof(Result<>)
                .MakeGenericType(resultType)
                .GetMethod(nameof(Result<object>.Failure));

            if (failureMethod is not null)
            {
                return (TResponse)failureMethod.Invoke(
                    null,
                    [
                        Error.Validation(ErrorCodes.ValidationError, ErrorMessage, errors)
                    ])!;
            }
        }
        else if (typeof(TResponse) == typeof(Result))
        {
            return (TResponse)(object)Result.Failure(Error.Validation(ErrorCodes.ValidationError, ErrorMessage, errors));
        }

        throw new CaravelException(Error.Validation(ErrorCodes.ValidationError, ErrorMessage, errors));
    }
}