using System;

namespace Caravel.Exceptions
{
    /// <summary>
    /// OperationCancelledException should be thrown when a determined operation was cancelled by the actor.
    /// </summary>
    public class OperationCancelledException: CaravelException
    {
        public OperationCancelledException(Error error, Exception? inner = null) : base(error, inner){}
        public OperationCancelledException(Error error, string message) : base(error, message){}
    }
}