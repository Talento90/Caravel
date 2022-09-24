using System.Collections.Generic;

namespace Caravel.Entities
{
    public interface IAggregateRoot : IEntity
    {
        void AddEvent(IDomainEvent domainEvent);
        IEnumerable<IDomainEvent> Events { get; }
    }
}