using System;
using Caravel.Errors;

namespace Caravel.Exceptions
{
    /// <summary>
    /// ConflictException should be thrown when there is a conflict.
    /// </summary>
    public class ConflictException : CaravelException
    {
        public ConflictException(Error error, Exception? innerException = null) : base(error, innerException){}
    }
}