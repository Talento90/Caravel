using System;

namespace Caravel.Entities
{
    public interface IEntity
    {
        public Ulid Id { get; set; }
    }
}