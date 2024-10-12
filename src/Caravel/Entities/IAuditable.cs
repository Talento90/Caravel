using System;

namespace Caravel.Entities
{
    public interface IAuditable
    {
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset? UpdatedAt { get; set; }
        public Ulid CreatedBy { get; set; }
        public Ulid? UpdatedBy { get; set; }
    }
}