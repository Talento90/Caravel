namespace Caravel.Exceptions;

/// <summary>
/// ValidationException should be thrown when any validation fails. 
/// </summary>
public class ValidationException : CaravelException
{
    public IDictionary<string, string[]> Errors { get; }

    public ValidationException(string message, IDictionary<string, string[]> errors) : base(message)
    {
        Errors = errors;
    }
}