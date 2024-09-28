namespace Caravel.MediatR.Idempotent;

public interface IIdempotentService
{
    Task<bool> RequestExistsAsync(Guid requestId, CancellationToken cancellationToken = default);
    Task CreateRequestAsync(IIdempotentRequest request, CancellationToken cancellationToken = default);
}