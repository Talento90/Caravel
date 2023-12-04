using System;

namespace Caravel.Entities
{
    public interface IAuditable
    {
        public DateTimeOffset Created { get; set; }
        public DateTimeOffset Updated { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid UpdatedBy { get; set; }
    }
}