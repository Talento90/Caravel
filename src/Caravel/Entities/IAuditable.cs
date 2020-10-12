using System;

namespace Caravel.Entities
{
    public interface IAuditable
    {
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid UpdatedBy { get; set; }
    }
}