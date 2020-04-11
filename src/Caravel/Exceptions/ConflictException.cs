using System;

namespace Caravel.Exceptions
{
    /// <summary>
    /// ConflictException should be thrown when there is a conflict.
    /// </summary>
    public class ConflictException : CaravelException
    {
        public ConflictException(Error error, Exception? innerException = null) : base(error, innerException){}
        
        public ConflictException(Error error, string message) : base(error, message){}
    }
}