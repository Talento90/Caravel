using System;

namespace Caravel.Exceptions
{
    public class CaravelException : Exception
    {
        public Error Error { get; }

        public CaravelException(Exception? innerException = null) : base(Errors.Error.Message, innerException)
        {
            Error = Error;
        }
        
        public CaravelException(Error error, Exception? innerException = null) : base(error.Message, innerException)
        {
            Error = error;
        }
        
        public CaravelException(Error error, string message) : base(message)
        {
            Error = error;
        }
    }
}