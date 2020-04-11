using System;

namespace Caravel.Exceptions
{
    /// <summary>
    /// NotFoundException should be thrown when a resource/entity does not exist.
    /// </summary>
    public class NotFoundException : CaravelException
    {
        public NotFoundException(Error error, Exception? innerException = null) : base(error, innerException){}
        public NotFoundException(Error error, string message) : base(error, message){}
    }
}