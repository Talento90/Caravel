using System;

namespace Caravel.Entities
{
    public interface IDomainEvent
    {
        Ulid Id { get; }
        string Name { get; }
        DateTimeOffset EventDate { get; }
    }
}