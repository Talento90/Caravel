using System;

namespace Caravel.Exceptions
{
    public class OperationCancelledException: CaravelException
    {
        public OperationCancelledException(Error error, Exception? inner = null) : base(error, inner){}
        public OperationCancelledException(Error error, string message) : base(error, message){}
    }
}