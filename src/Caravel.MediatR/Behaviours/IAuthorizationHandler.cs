using MediatR;

namespace Caravel.MediatR.Behaviours
{
    public interface IAuthorizationHandler<TRequest> : IRequestHandler<TRequest, AuthorizationResult> where TRequest : IRequest<AuthorizationResult>
    {
    }
}