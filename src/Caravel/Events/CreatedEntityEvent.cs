using Caravel.Entities;

namespace Caravel.Events;

public class CreatedEntityEvent : IDomainEvent
{
    public Ulid Id { get; }
    public string Name => $"{After.GetType().Name}.CreatedAt";
    public DateTimeOffset EventDate { get; set; }
    public IEntity After { get; }

    public CreatedEntityEvent(IEntity createdEntity)
    {
        Id = Ulid.NewUlid();
        After = createdEntity;
        EventDate = DateTimeOffset.UtcNow;
    }
}