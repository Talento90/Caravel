using Caravel.Entities;

namespace Caravel.Events;

public class DeletedEntityEvent : IDomainEvent
{
    public Ulid Id { get; }
    public string Name => $"{Before.GetType().Name}.Deleted";
    public DateTimeOffset EventDate { get; set; }
    public IEntity Before { get; }

    public DeletedEntityEvent(IEntity createdEntity)
    {
        Id = Ulid.NewUlid();
        Before = createdEntity;
        EventDate = DateTimeOffset.UtcNow;
    }
}