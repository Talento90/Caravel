namespace Caravel.Errors;

/// <summary>
/// Error class represents the application error model.
/// All errors should use this class in order to provide consistency.
/// </summary>
public sealed class Error
{
    public string Code { get; }
    public ErrorType Type { get; }
    public string Message { get; }
    public string? Detail { get; }
    public ErrorSeverity Severity { get; }
    public List<ValidationError> ValidationErrors { get; } = new List<ValidationError>();

    public Error(string code, ErrorType type, string message, string? detail = null, ErrorSeverity severity = ErrorSeverity.Low)
    {
        Code = code;
        Type = type;
        Message = message;
        Detail = detail;
        Severity = severity;
    }

    public Error WithErrors(IEnumerable<ValidationError> errors)
    {
        ValidationErrors.AddRange(errors);
        return this;
    }

    public static readonly Error None = new ("none", ErrorType.None, "None");

    public static Error Validation(string code, string message, string? detail = null, ErrorSeverity severity = ErrorSeverity.Low)
    {
        return new Error(code, ErrorType.Validation, message, detail, severity);
    }
    
    public static Error Validation(string code, string message, IEnumerable<ValidationError> errors, string? detail = null, ErrorSeverity severity = ErrorSeverity.Low)
    {
        return new Error(code, ErrorType.Validation, message, detail, severity).WithErrors(errors);
    }
    public static Error Conflict(string code, string message, string? detail = null, ErrorSeverity severity = ErrorSeverity.Low)
    {
        return new Error(code, ErrorType.Conflict, message, detail, severity);
    }
    public static Error Unauthorized(string code, string message, string? detail = null, ErrorSeverity severity = ErrorSeverity.Low)
    {
        return new Error(code, ErrorType.Unauthorized, message, detail, severity);
    }
    public static Error NotFound(string code, string message, string? detail = null, ErrorSeverity severity = ErrorSeverity.Low)
    {
        return new Error(code, ErrorType.NotFound, message, detail, severity);
    }
    public static Error Unauthenticated(string code, string message, string? detail = null, ErrorSeverity severity = ErrorSeverity.Low)
    {
        return new Error(code, ErrorType.Unauthenticated, message, detail, severity);
    }
    public static Error Internal(string code, string message, string? detail = null, ErrorSeverity severity = ErrorSeverity.Low)
    {
        return new Error(code, ErrorType.Internal, message, detail, severity);
    }
}