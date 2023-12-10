using Caravel.Errors;

namespace Caravel.AspNetCore.Http
{
    public static class HttpExtensions
    {
        public static HttpError ToProblemDetails(this Error error)
        {
            return new HttpError(error);
        }

        public static IResult ToHttpResult(this Error error)
        {
            return error.Type switch
            {
                ErrorType.Validation => Results.BadRequest(error.ToProblemDetails()),
                ErrorType.Permission => Results.Forbid(),
                ErrorType.Unauthorized => Results.Unauthorized(),
                ErrorType.Conflict => Results.Conflict(error.ToProblemDetails()),
                ErrorType.NotFound => Results.NotFound(error.ToProblemDetails()),
                _ => Results.StatusCode(StatusCodes.Status500InternalServerError)
            };
        }
    }
}