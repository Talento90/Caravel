using MediatR;

namespace Caravel.MediatR.Behaviours
{
    public interface IAuthorizationRequirement : IRequest<AuthorizationResult> { }
}