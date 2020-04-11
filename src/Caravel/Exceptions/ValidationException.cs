using System;
using System.Collections.Generic;

namespace Caravel.Exceptions
{
    /// <summary>
    /// ValidationException should be thrown when any validation fails. 
    /// </summary>
    public class ValidationException : CaravelException
    {
        public IDictionary<string, string[]> Errors { get; }

        public ValidationException(Error error, Exception? innerException = null) : base(error, innerException)
        {
            Errors = new Dictionary<string, string[]>();
        }

        public ValidationException(Error error, IDictionary<string, string[]> errors, Exception? innerException = null) : base(error, innerException)
        {
            Errors = errors ?? new Dictionary<string, string[]>();;
        }

        public ValidationException(Error error, string message) : base(error, message)
        {
            Errors = new Dictionary<string, string[]>();
        }
    }
}