using Caravel.Errors;
using Caravel.Functional;
using MediatR;

namespace Caravel.MediatR;

public interface IIdempotentService
{
    Task<bool> RequestExistsAsync(Guid requestId, CancellationToken cancellationToken = default);
    Task CreateRequestAsync(IIdempotentRequest request, CancellationToken cancellationToken = default);
}


public class IdempotentRequestBehaviour <TRequest, TResponse> : IPipelineBehavior<TRequest, Result<TResponse>>
    where TRequest : IIdempotentRequest
{
    private readonly IIdempotentService _idempotentService;

    public IdempotentRequestBehaviour(IIdempotentService idempotentService)
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