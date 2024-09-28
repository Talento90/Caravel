using Caravel.Errors;

namespace Caravel.AspNetCore.Http
{
    public static class ApiProblemDetailsExtensions
    {
        public static ApiProblemDetails ToProblemDetails(this Error error)
        {
            return new ApiProblemDetails(error);
        }

        public static IResult ToApiProblemDetailsResult(this Error error)
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