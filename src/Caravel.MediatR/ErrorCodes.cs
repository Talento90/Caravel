namespace Caravel.MediatR;

public static class ErrorCodes
{
    public const string ValidationError = "validation_request";
    public const string IdempotentError = "duplicate_request";
}