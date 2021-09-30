using System;
using System.Threading;
using System.Threading.Tasks;

namespace Caravel.MediatR.Security
{
    public interface IAuthorizer
    {
        Task<bool> IsInRoleAsync(Guid userId, string role, CancellationToken ct = default);
        Task<bool> AuthorizeAsync(Guid userId, string policy, CancellationToken ct = default);
    }
}