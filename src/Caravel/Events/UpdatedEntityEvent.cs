using Caravel.Entities;

namespace Caravel.Events;

public class UpdatedEntityEvent : IDomainEvent
{
    public Ulid Id { get; }
    public string Name => $"{Before.GetType().Name}.UpdatedAt";
    public DateTimeOffset EventDate { get; set; }
    public IEntity Before { get; }
    public IEntity After { get; }

    public UpdatedEntityEvent(IEntity beforeEntity, IEntity afterEntity)
    {
        Id = Ulid.NewUlid();
        Before = beforeEntity;
        After = afterEntity;
        EventDate = DateTimeOffset.UtcNow;
    }
}