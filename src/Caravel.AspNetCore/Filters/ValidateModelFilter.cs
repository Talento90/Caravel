using System.Net;
using Caravel.AspNetCore.Http;
using Caravel.Errors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Caravel.AspNetCore.Filters;

/// <summary>
/// ValidateModelFilter validates the <see cref="ModelStateDictionary"/>
/// Returns 400 if not valid.
/// </summary>
public class ValidateModelFilter : IActionFilter
{
    public void OnActionExecuting(ActionExecutingContext filterContext)
    {
        if (!filterContext.ModelState.IsValid)
        {
            var errors = filterContext.ModelState
                .Where(x => x.Value?.Errors.Count > 0)
                .ToDictionary(kvp =>
                        kvp.Key,
                    kvp => kvp.Value!.Errors.Select(e => e.ErrorMessage).ToArray()
                );

            var httpError = new HttpError(
                    filterContext.HttpContext,
                    HttpStatusCode.BadRequest,
                    new Error("invalid_fields", "Payload contains invalid fields.")
                )
                .SetErrors(errors);

            filterContext.Result = new BadRequestObjectResult(httpError);
        }
    }

    public void OnActionExecuted(ActionExecutedContext filterContext)
    {
    }
}