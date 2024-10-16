using System;

namespace Caravel.Entities
{
    public class Entity : IEntity, IAuditable
    {
        public Ulid Id { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset? UpdatedAt { get; set; }
        public Ulid CreatedBy { get; set; }
        public Ulid? UpdatedBy { get; set; }

        public Entity()
        {
            Id = Ulid.NewUlid();
            CreatedAt = DateTimeOffset.UtcNow;
            UpdatedAt = DateTimeOffset.UtcNow;
        }
    }
}