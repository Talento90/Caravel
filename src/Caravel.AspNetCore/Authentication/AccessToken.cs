namespace Caravel.AspNetCore.Authentication;

public record AccessToken(string Token, int ExpiresIn);