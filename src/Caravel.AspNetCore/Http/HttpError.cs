using System.Collections.Generic;
using System.Linq;
using System.Net;
using Caravel.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Caravel.AspNetCore.Http
{
    /// <summary>
    /// HttpError represents all the http errors in the application.
    /// It extends the <see cref="ProblemDetails"/> which follow the https://tools.ietf.org/html/rfc7807 specification.
    /// All http error responses should return this class in order to provide consistency.
    /// </summary>
    public class HttpError : ProblemDetails
    {
        /// <summary>
        /// Trace Identifier to help tracking the problem.
        /// </summary>
        public string? TraceId { get; set; }
        /// <summary>
        /// Error code must a well known code in the <see cref="Errors"/> class.
        /// </summary>
        public int Code { get; set; }
        
        public HttpError()
        {
        }

        public HttpError(HttpContext context, HttpStatusCode code, CaravelException ex)
        {
            Status = (int) code;
            Title = ex.Error.Message;
            Detail = ex.Message;
            Code = ex.Error.Code;
            TraceId = context.TraceIdentifier;
            Instance = context.Request?.Path;
        }

        /// <summary>
        /// Set the errors.
        /// </summary>
        /// <param name="errors"></param>
        /// <returns></returns>
        public HttpError SetErrors(IDictionary<string, string[]> errors)
        {
            if (errors != null && errors.Any())
            {
                Extensions["errors"] = errors;    
            }
            
            return this;
        }
    }
}