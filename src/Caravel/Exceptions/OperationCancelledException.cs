using System;

namespace Caravel.Exceptions
{
    public class OperationCancelledException: CaravelException
    {
        public OperationCancelledException(Error error, Exception? inner = null) : base(error, inner)
        {
        }
    }
}