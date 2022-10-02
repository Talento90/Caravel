using System;
using Caravel.Entities;

namespace Caravel.Events;

public class UpdatedEntityEvent : IDomainEvent
{
    public Guid Id { get; }
    public string Name => $"{Before.GetType().Name}.Updated";
    public DateTime EventDate { get; set; }
    public IEntity Before { get; }
    public IEntity After { get; }

    public UpdatedEntityEvent(IEntity beforeEntity, IEntity afterEntity)
    {
        Id = Guid.NewGuid();
        Before = beforeEntity;
        After = afterEntity;
        EventDate = DateTime.UtcNow;
    }
}