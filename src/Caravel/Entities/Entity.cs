using System;

namespace Caravel.Entities
{
    public class Entity : IEntity, IAuditable
    {
        public Guid Id { get; set; }
        public DateTimeOffset Created { get; set; }
        public DateTimeOffset Updated { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid UpdatedBy { get; set; }

        public Entity()
        {
            Id = Guid.NewGuid();
            Created = DateTimeOffset.UtcNow;
            Updated = DateTimeOffset.UtcNow;
        }
    }
}