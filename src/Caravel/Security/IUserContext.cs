using Caravel.Functional;

namespace Caravel.Security;

public interface IUserContext
{
    Result<string> UserId();
    Result<string> TenantId();
    bool HasPermission(string permission);
}