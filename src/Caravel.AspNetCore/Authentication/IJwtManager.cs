using System.Collections.Generic;
using System.Security.Claims;

namespace Caravel.AspNetCore.Authentication
{
    public interface IJwtManager
    {
        AccessToken GenerateAccessToken(string id, string username, IEnumerable<Claim> claims);
        ClaimsPrincipal? GetClaims(string token, string signingKey);
    }
}