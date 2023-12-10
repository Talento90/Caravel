namespace Caravel.Errors;

public record ValidationError(string Identifier, string[] Errors);