using System.Security.Claims;

namespace Caravel.ApplicationContext;

public static class ApplicationContextExtensions
{
    
    /// <summary>
    /// Get the current user id.
    /// </summary>
    /// <param name="principal"></param>
    /// <returns>Return the current user id or null if does not exists.</returns>
    public static Guid? Id(this ClaimsPrincipal principal)
    {
        var uid = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

        if (Guid.TryParse(uid, out var result))
            return result;

        return null;
    }
        
    /// <summary>
    /// Get the current tenant id.
    /// </summary>
    /// <param name="appContext"></param>
    /// <returns>Return the current user id or null if does not exists.</returns>
    public static Guid? TenantId(this ApplicationContext appContext)
    {
        var uid = appContext.User.Claims.FirstOrDefault(c => c.Type == "tenant_id")?.Value;

        if (Guid.TryParse(uid, out var result))
            return result;

        return null;
    }
}