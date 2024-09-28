using Caravel.Security;

namespace Caravel.AspNetCore.Security;

public class UserContext : IUserContext
{
    private readonly IHttpContextAccessor _contextAccessor;

    public UserContext(IHttpContextAccessor contextAccessor)
    {
        _contextAccessor = contextAccessor;
    }

    public Guid? UserId => _contextAccessor.HttpContext?.User.UserId();

    public Guid? TenantId => _contextAccessor.HttpContext?.User.TenantId();

    public bool HasPermission(string permission)
    {
        return _contextAccessor.HttpContext?.User.HasPermission(permission) ?? false;
    }
}