using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.Json.Serialization;
using Caravel.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Caravel.AspNetCore.Http
{
    public class HttpError : ProblemDetails
    {
        public string? TraceId { get; set; }
        public int Code { get; set; }
        [JsonIgnore]
        public CaravelException? Exception { get; }

        public HttpError()
        {
        }

        public HttpError(HttpContext context, HttpStatusCode code, CaravelException ex)
        {
            Exception = ex;
            Status = (int) code;
            Title = ex.Error.Message;
            Detail = ex.Message;
            Code = ex.Error.Code;
            TraceId = context.TraceIdentifier;
            Instance = context.Request?.Path;
        }

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