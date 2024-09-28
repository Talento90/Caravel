namespace Caravel.MediatR.Idempotent;

public interface IIdempotentRequest
{
    public Guid IdempotentKey { get; set; }
}