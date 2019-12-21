using System.Linq;
using System.Net;
using Caravel.Exceptions;
using Caravel.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Caravel.AspNetCore.Filters
{
    public class ValidateModelFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!filterContext.ModelState.IsValid)
            {
                var errors = filterContext.ModelState
                    .Where(x => x.Value.Errors.Count > 0)
                    .ToDictionary(kvp =>
                            kvp.Key,
                        kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
                    );

                var httpError = new HttpError(
                        "Bad Request",
                        HttpStatusCode.BadRequest,
                        new ValidationException(Errors.FieldsValidation, errors),
                        filterContext.HttpContext.TraceIdentifier
                    )
                    .SetErrors(errors);

                filterContext.Result = new BadRequestObjectResult(httpError);
            }
        }

        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
        }
    }
}


