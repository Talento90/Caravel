using System;

namespace Caravel.Exceptions
 {
     public class UnauthorizedException : CaravelException
     {
         public UnauthorizedException(Error error, Exception? innerException = null) : base(error, innerException)
         {
         }
     }
 }