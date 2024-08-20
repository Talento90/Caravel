namespace Caravel.MediatR;

public interface IIdempotentRequest
{
    public Guid IdempotentKey { get; set; }
}