using System.Collections.Generic;

namespace Caravel.MediatR.Behaviours
{
    public abstract class AbstractRequestAuthorize<TRequest> : IAuthorize<TRequest>
    {
        private readonly HashSet<IAuthorizationRequirement> _requirements = new HashSet<IAuthorizationRequirement>();

        public IEnumerable<IAuthorizationRequirement> Requirements => _requirements;

        protected void UseRequirement(IAuthorizationRequirement requirement)
        {
            _requirements.Add(requirement);
        }

        public abstract void BuildPolicy(TRequest request);
        
    }
}