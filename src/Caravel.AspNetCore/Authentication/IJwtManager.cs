using System.Security.Claims;

namespace Caravel.AspNetCore.Authentication
{
    public interface IJwtManager
    {
        AccessToken GenerateAccessToken(string id, string username, string[] roles = default);
        ClaimsPrincipal? GetClaims(string token, string signingKey);
    }
}