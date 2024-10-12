using System.Security.Claims;
using Microsoft.IdentityModel.JsonWebTokens;

namespace Caravel.AspNetCore.Security;

public static class ClaimsPrincipalExtensions
{
    private const string TenantClaim = "tenant_id";
    private const string ScopeClaim = "scope";

    /// <summary>
    /// Get the current user id.
    /// </summary>
    /// <param name="principal"></param>
    /// <returns>Return the current user id or null if does not exists.</returns>
    public static string? UserId(this ClaimsPrincipal principal)
    {
        return principal.Claim(JwtRegisteredClaimNames.Sub) ?? principal.Claim(ClaimTypes.NameIdentifier);
    }
    
    /// <summary>
    /// Get Claim from User.
    /// </summary>
    /// <param name="principal"></param>
    /// <param name="claim"></param>
    /// <returns></returns>
    public static string? Claim(this ClaimsPrincipal principal, string claim)
    {
        return principal.Claims.FirstOrDefault(c => c.Type == claim)?.Value;
    }
    
    /// <summary>
    /// Get Claim from User.
    /// </summary>
    /// <param name="principal"></param>
    /// <param name="permission">Permission to validate</param>
    /// <returns></returns>
    public static bool HasPermission(this ClaimsPrincipal principal, string permission)
    {
        var scope = principal.FindFirst(ScopeClaim);
        return scope is not null && scope.Value.Contains(permission);
    }
        
    /// <summary>
    /// Get the current tenant id.
    /// </summary>
    /// <param name="principal"></param>
    /// <returns>Return the current user id or null if does not exists.</returns>
    public static string? TenantId(this ClaimsPrincipal principal)
    {
        return principal.Claim(TenantClaim);
    }
}