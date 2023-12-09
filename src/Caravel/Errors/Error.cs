namespace Caravel.Errors;

/// <summary>
/// Error class represents the application error model.
/// All errors should use this class in order to provide consistency.
/// </summary>
public sealed record Error<TError>(
    TError Code,
    ErrorType Type,
    string Message,
    string? Detail = null);