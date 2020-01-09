using System;
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
        public string? InnerMessage { get; set; }
        public int Code { get; set; }

        [JsonIgnore]
        public CaravelException Exception { get; }

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

            if (!Env.IsProductionEnvironment())
            {
                InnerMessage = string.Join("\n", ExtractExceptionMessages(ex.InnerException));
            }
        }

        public HttpError SetErrors(IDictionary<string, string[]> errors)
        {
            Extensions["errors"] = errors;
            return this;
        }

        private static IEnumerable<string> ExtractExceptionMessages(Exception? ex)
        {
            var errors = new List<string>();

            while (ex != null)
            {
                errors.Add(ex.Message);
                ex = ex.InnerException;
            }

            return errors;
        }
    }
}