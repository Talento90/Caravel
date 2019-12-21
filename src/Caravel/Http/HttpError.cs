using System;
using System.Collections.Generic;
using System.Net;
using Caravel.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace Caravel.Http
{
    public class HttpError : ProblemDetails
    {
        public string? CorrelationId { get; set; }
        public string? InnerMessage { get; set; }
        public int Code { get; set; }

        public HttpError()
        {
        }

        public HttpError(string title, HttpStatusCode code, CaravelException ex, string correlationId)
        {
            Status = (int) code;
            Title = title;
            Detail = ex.Message;
            Code = ex.Error.Code;
            CorrelationId = correlationId;

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