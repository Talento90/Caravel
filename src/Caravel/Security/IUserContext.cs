namespace Caravel.Security;

public interface IUserContext
{   
    Guid? UserId { get; }
    Guid? TenantId { get; }
    bool HasPermission(string permission);
}