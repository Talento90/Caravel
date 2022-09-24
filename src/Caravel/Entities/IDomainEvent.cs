using System;

namespace Caravel.Entities
{
    public interface IDomainEvent
    {
        Guid Id { get; }
        string Name { get; }
        DateTime EventDate { get; set; }
    }
}