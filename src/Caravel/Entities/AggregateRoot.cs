using System.Collections.Generic;

namespace Caravel.Entities
{
    public abstract class AggregateRoot : Entity, IAggregateRoot
    {
        private readonly List<IDomainEvent> _domainEvents;

        protected AggregateRoot(List<IDomainEvent> domainEvents)
        {
            _domainEvents = domainEvents;
        }
        
        public void AddEvent(IDomainEvent domainEvent)
        {
            _domainEvents.Add(domainEvent);
        }

        public IEnumerable<IDomainEvent> Events => _domainEvents.AsReadOnly();

        public void ClearEvents()
        {
            _domainEvents.Clear();
        }
    }
}