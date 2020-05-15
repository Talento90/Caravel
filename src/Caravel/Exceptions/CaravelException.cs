using System;
using Caravel.Errors;

namespace Caravel.Exceptions
{
    /// <summary>
    /// CaravelException is the base application exception.
    /// All exceptions should extend this class in order to provide consistency in our application.
    /// </summary>
    public class CaravelException : Exception
    {
        public Error Error { get; }
        
        public CaravelException(Error error, Exception? innerException = null) : base(error.Message, innerException)
        {
            Error = error;
        }
    }
}