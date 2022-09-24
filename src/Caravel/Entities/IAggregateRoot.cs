using System.Collections.Generic;

namespace Caravel.Entities
{
    public interface IAggregateRoot : IEntity
    {
        void AddEvent(IDomainEvent domainEvent);
        void ClearEvents();
        IEnumerable<IDomainEvent> Events { get; }
    }
}