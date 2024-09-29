using Caravel.Errors;
using Caravel.Functional;
using MediatR;

namespace Caravel.MediatR.Idempotent;

public sealed class IdempotentPipelineBehaviour <TRequest, TResponse> : IPipelineBehavior<TRequest, Result<TResponse>>
    where TRequest : IIdempotentRequest
{
    private readonly IIdempotentService _idempotentService;

    public IdempotentPipelineBehaviour(IIdempotentService idempotentService)
    {
        _idempotentService = idempotentService;
    }


    public async Task<Result<TResponse>> Handle(TRequest request, RequestHandlerDelegate<Result<TResponse>> next, CancellationToken cancellationToken)
    {
        var isDuplicatedRequest = await _idempotentService.RequestExistsAsync(request.IdempotentKey, cancellationToken);

        if (isDuplicatedRequest)
        {
            return Error.Conflict(
                ErrorCodes.IdempotentError,
                "Duplicate request.",
                $"Duplicate request with Idempotent Key {request.IdempotentKey}.");
        }

        await _idempotentService.CreateRequestAsync(request, cancellationToken);
        
        return await next();
    }
}