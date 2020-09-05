using System.Collections.Generic;

namespace Caravel.MediatR.Behaviours
{
    public interface IAuthorize<T>
    {
        IEnumerable<IAuthorizationRequirement> Requirements { get; }
        void BuildPolicy(T instance);
    }
}