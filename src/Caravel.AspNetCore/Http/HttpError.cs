using System.Text.Json.Serialization;
using Caravel.Errors;
using Microsoft.AspNetCore.Mvc;

namespace Caravel.AspNetCore.Http;

public class HttpError : ProblemDetails
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyOrder(-6)]
    public string? Code { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyOrder(-7)]
    public Dictionary<string, IEnumerable<string>>? Errors { get; set; }

    public HttpError()
    {
        
    }
    
    public HttpError(Error error)
    {
        Title = error.Message;
        Detail = error.Detail;
        Code = error.Code;
        Status = MapErrorTypeToHttpStatusCode(error.Type);
        Errors = error.ValidationErrors
            .GroupBy((ValidationError validationError) => validationError.Identifier)
            .ToDictionary(
                keySelector => keySelector.Key,
                valueSelector => valueSelector.SelectMany(ve => ve.Errors.ToArray()));
    }
    
    private static int MapErrorTypeToHttpStatusCode(ErrorType type)
    {
        return type switch
        {
            ErrorType.Validation => StatusCodes.Status400BadRequest,
            ErrorType.Permission => StatusCodes.Status403Forbidden,
            ErrorType.Unauthorized => StatusCodes.Status401Unauthorized,
            ErrorType.Conflict => StatusCodes.Status409Conflict,
            ErrorType.NotFound => StatusCodes.Status404NotFound,
            _ => StatusCodes.Status500InternalServerError
        };
    }
}