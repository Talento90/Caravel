using System.Collections.Generic;
using System.Net;
using System.Text.Json.Serialization;
using Caravel.Exceptions;
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

        public HttpError(string title, HttpStatusCode code, CaravelException ex, string traceId)
        {
            Exception = ex;
            Status = (int) code;
            Title = title;
            Detail = ex.Message;
            Code = ex.Error.Code;
            TraceId = traceId;
        }

        public HttpError SetErrors(IDictionary<string, string[]> errors)
        {
            Extensions["errors"] = errors;
            return this;
        }
    }
}