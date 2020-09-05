using System.Security.Claims;

namespace Caravel.AspNetCore.Authentication
{
    public interface IJwtManager
    {
        AccessToken GenerateAccessToken(string id, string username, string[] roles);
        ClaimsPrincipal? GetClaims(string token, string signingKey);
    }
}