using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Caravel.Errors;
using Caravel.Exceptions;
using MediatR;

namespace Caravel.MediatR.Behaviours
{
    public class RequestAuthorizationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        private readonly IEnumerable<IAuthorize<TRequest>> _authorizes;
        private readonly IMediator _mediator;

        public RequestAuthorizationBehavior(IEnumerable<IAuthorize<TRequest>> authorizes, IMediator mediator)
        {
            _authorizes = authorizes;
            _mediator = mediator;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            var requirements = new List<IAuthorizationRequirement>();

            foreach (var authorize in _authorizes)
            {
                authorize.BuildPolicy(request);
                requirements.AddRange(authorize.Requirements);
            }

            foreach (var requirement in requirements.Distinct())
            {
                var result = await _mediator.Send(requirement, cancellationToken);
                
                if (!result.IsAuthorized)
                    throw new UnauthorizedException(new Error("unauthorized", result.FailureMessage ?? "User is unauthorized."));
            }

            return await next();
        }
    }
}