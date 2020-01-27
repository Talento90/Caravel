using System;

namespace Caravel.Exceptions
{
    public class PermissionException : CaravelException
    {
        public PermissionException(Error error, Exception? innerException = null) : base(error, innerException){}
        public PermissionException(Error error, string message) : base(error, message){}
    }
}