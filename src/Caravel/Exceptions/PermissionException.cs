using System;
using Caravel.Errors;

namespace Caravel.Exceptions
{
    /// <summary>
    /// PermissionException should be thrown when application knows the user but does
    /// not have permission to execute the operation.
    /// </summary>
    public class PermissionException : CaravelException
    {
        public PermissionException(Error error, Exception? innerException = null) : base(error, innerException){}
    }
}