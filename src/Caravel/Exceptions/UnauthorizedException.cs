using System;

namespace Caravel.Exceptions
 {
     /// <summary>
     /// UnauthorizedException should be thrown when application does not know the user.
     /// </summary>
     public class UnauthorizedException : CaravelException
     {
         public UnauthorizedException(Error error, Exception? innerException = null) : base(error, innerException){}
         public UnauthorizedException(Error error, string message) : base(error, message){}
     }
 }