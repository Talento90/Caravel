using System;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Caravel.AppContext;
using Caravel.Errors;
using Caravel.Exceptions;
using Caravel.MediatR.Security;
using MediatR;

namespace Caravel.MediatR.Behaviours
{
    public class AuthorizationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly AppContext.AppContext _appContext;
        private readonly IAuthorizer _authorizer;

        public AuthorizationBehaviour(IAppContextAccessor appContextAccessor, IAuthorizer authorizer)
        {
            _appContext = appContextAccessor.Context;
            _authorizer = authorizer;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken ct,
            RequestHandlerDelegate<TResponse> next)
        {
            if (!_appContext.UserId.HasValue)
            {
                throw new UnauthorizedException(new Error("authentication", "User is not authenticated."));
            }

            var authorizeAttributes = request?.GetType().GetCustomAttributes<AuthorizeAttribute>().ToList();

            if (authorizeAttributes is null || !authorizeAttributes.Any())
                return await next();

            // Role-based authorization
            var authorizeAttributesWithRoles =
                authorizeAttributes.Where(a => !string.IsNullOrWhiteSpace(a.Roles)).ToList();

            if (authorizeAttributesWithRoles.Any())
            {
                var authorized = false;

                foreach (var roles in authorizeAttributesWithRoles.Select(a => a.Roles.Split(',')))
                {
                    foreach (var role in roles)
                    {
                        var isInRole =
                            await _authorizer.IsInRoleAsync(_appContext.UserId.Value, role.Trim(), ct);

                        if (isInRole)
                        {
                            authorized = true;
                            break;
                        }
                    }
                }

                if (!authorized)
                {
                    throw new PermissionException(new Error("permission", "User is not have the right role."));
                }
            }

            // Policy-based authorization
            var authorizeAttributesWithPolicies =
                authorizeAttributes.Where(a => !string.IsNullOrWhiteSpace(a.Policy)).ToList();

            if (authorizeAttributesWithPolicies.Any())
            {
                foreach (var policy in authorizeAttributesWithPolicies.Select(a => a.Policy))
                {
                    var authorized =
                        await _authorizer.AuthorizeAsync(_appContext.UserId.Value, policy, ct);

                    if (!authorized)
                    {
                        throw new PermissionException(new Error("permission", "User does not have the right policy."));
                    }
                }
            }

            return await next();
        }
    }
}