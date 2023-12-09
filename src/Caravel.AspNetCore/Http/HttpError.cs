using Caravel.Errors;
using Microsoft.AspNetCore.Mvc;

namespace Caravel.AspNetCore.Http;

public class HttpError<T> : ProblemDetails
{
    public T Code { get; }

    public HttpError(Error<T> error)
    {
        Title = error.Message;
        Detail = error.Detail;
        Code = error.Code;
        Status = MapErrorTypeToHttpStatusCode(error.Type);
    }
    
    private static int MapErrorTypeToHttpStatusCode(ErrorType type)
    {
        return type switch
        {
            ErrorType.Validation => StatusCodes.Status400BadRequest,
            ErrorType.Permission => StatusCodes.Status403Forbidden,
            ErrorType.Conflict => StatusCodes.Status409Conflict,
            ErrorType.NotFound => StatusCodes.Status404NotFound,
            _ => StatusCodes.Status500InternalServerError
        };
    }
}