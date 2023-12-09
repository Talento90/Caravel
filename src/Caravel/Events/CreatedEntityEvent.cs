using Caravel.Entities;

namespace Caravel.Events;

public class CreatedEntityEvent : IDomainEvent
{
    public Guid Id { get; }
    public string Name => $"{After.GetType().Name}.Created";
    public DateTimeOffset EventDate { get; set; }
    public IEntity After { get; }

    public CreatedEntityEvent(IEntity createdEntity)
    {
        Id = Guid.NewGuid();
        After = createdEntity;
        EventDate = DateTimeOffset.UtcNow;
    }
}