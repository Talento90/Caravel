namespace Caravel.MediatR;

public static class ErrorCodes
{
    public const string ValidationError = "invalid_fields";
    public const string IdempotentError = "duplicate_request";
}