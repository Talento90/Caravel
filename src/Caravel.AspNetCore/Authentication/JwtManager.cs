using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using Caravel.Clock;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Caravel.AspNetCore.Authentication
{
    public sealed class JwtManager : IJwtManager
    {
        private readonly JwtIssuerSettings _jwtSettings;
        private readonly IClock _clock;
        private readonly JwtSecurityTokenHandler _jwtSecurityTokenHandler;

        public JwtManager(IOptions<JwtIssuerSettings> jwtSettings, IClock clock)
        {
            _jwtSettings = jwtSettings.Value;
            _clock = clock;
            _jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
        }

        public AccessToken GenerateAccessToken(string id, string username, IEnumerable<Claim> claims)
        {
            var identity = new ClaimsIdentity(new GenericIdentity(username, "Token"), new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, id),
                new Claim(JwtRegisteredClaimNames.Sub, username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, _clock.NowOffsetUtc().ToUnixTimeSeconds().ToString(),
                    ClaimValueTypes.Integer64)
            }.Concat(claims));
            
            var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_jwtSettings.IssuerSigningKey));

            var jwt = new JwtSecurityToken(
                _jwtSettings.Issuer,
                _jwtSettings.Audience,
                identity.Claims,
                _clock.NowUtc(),
                _clock.NowUtc().AddMinutes(_jwtSettings.ExpirationInMinutes),
                new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256)
            );

            return new AccessToken(
                _jwtSecurityTokenHandler.WriteToken(jwt),
                (int) TimeSpan.FromMinutes(_jwtSettings.ExpirationInMinutes).TotalSeconds
            );
        }

        public ClaimsPrincipal? GetClaims(string token, string signingKey)
        {
            var claims = _jwtSecurityTokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateAudience = true,
                ValidateIssuer = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signingKey)),
                ValidateLifetime = true
            }, out var securityToken);

            return !(securityToken is JwtSecurityToken) ? null : claims;
        }
    }
}