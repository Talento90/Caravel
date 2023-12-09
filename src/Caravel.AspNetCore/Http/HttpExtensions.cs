using System.Security.Claims;
using Caravel.AspNetCore.Authentication;
using Caravel.Errors;
using Microsoft.AspNetCore.Mvc;

namespace Caravel.AspNetCore.Http
{
    public static class HttpExtensions
    {
        

        public static HttpError<T> ToProblemDetails<T>(this Error<T> error)
        {
            return new HttpError<T>(error);
        }
    }
}