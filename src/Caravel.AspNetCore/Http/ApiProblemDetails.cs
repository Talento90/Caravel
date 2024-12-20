using System.Text.Json.Serialization;
using Caravel.Errors;
using Microsoft.AspNetCore.Mvc;

namespace Caravel.AspNetCore.Http;

public class ApiProblemDetails : ProblemDetails
{
    [JsonPropertyName("code")] public string? Code { get; set; }
    [JsonPropertyName("errors")] public Dictionary<string, IEnumerable<string>>? Errors { get; set; }

    public ApiProblemDetails()
    {
    }

    public ApiProblemDetails(Error error)
    {
        Title = error.Message;
        Detail = error.Detail;
        Code = error.Code;
        Status = MapErrorTypeToHttpStatusCode(error.Type);

        if (error.ValidationErrors.Count != 0)
        {
            Errors = error.ValidationErrors
                .GroupBy((ValidationError validationError) => validationError.Identifier)
                .ToDictionary(
                    keySelector => keySelector.Key,
                    valueSelector => valueSelector.SelectMany(ve => ve.Errors.ToArray()));
        }
    }

    private static int MapErrorTypeToHttpStatusCode(ErrorType type)
    {
        return type switch
        {
            ErrorType.Validation => StatusCodes.Status400BadRequest,
            ErrorType.Unauthorized => StatusCodes.Status403Forbidden,
            ErrorType.Unauthenticated => StatusCodes.Status401Unauthorized,
            ErrorType.Conflict => StatusCodes.Status409Conflict,
            ErrorType.NotFound => StatusCodes.Status404NotFound,
            _ => StatusCodes.Status500InternalServerError
        };
    }
}