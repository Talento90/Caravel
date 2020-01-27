using System;

namespace Caravel.Exceptions
{
    public class NotFoundException : CaravelException
    {
        public NotFoundException(Error error, Exception? innerException = null) : base(error, innerException){}
        public NotFoundException(Error error, string message) : base(error, message){}
    }
}