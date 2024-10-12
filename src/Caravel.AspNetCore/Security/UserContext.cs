using Caravel.Errors;
using Caravel.Functional;
using Caravel.Security;

namespace Caravel.AspNetCore.Security;

public class UserContext(IHttpContextAccessor contextAccessor) : IUserContext
{
    public Result<string> UserId()
    {
        var userId = contextAccessor.HttpContext?.User.UserId();

        return string.IsNullOrEmpty(userId)
            ? Error.Unauthorized("unauthorized_user", "User is not authenticated.")
            : Result<string>.Success(userId);
    }
    
    public Result<string> TenantId()
    {
        var tenantId = contextAccessor.HttpContext?.User.TenantId();

        return string.IsNullOrEmpty(tenantId)
            ? Error.Unauthorized("unauthorized_tenant", "Missing tenant identifier.")
            : Result<string>.Success(tenantId);
    }

    public bool HasPermission(string permission)
    {
        return contextAccessor.HttpContext?.User.HasPermission(permission) ?? false;
    }
}