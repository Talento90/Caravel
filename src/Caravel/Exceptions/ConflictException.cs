using System;

namespace Caravel.Exceptions
{
    public class ConflictException : CaravelException
    {
        public ConflictException(Error error, Exception? innerException = null) : base(error, innerException){}
    }
}