using System.Reflection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Caravel.AspNetCore.Endpoint;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddEndpointFeatures(this IServiceCollection services, Assembly assembly)
    {
        var serviceDescriptors = assembly
            .DefinedTypes
            .Where(type => type is {IsPublic: true, IsAbstract: false, IsInterface: false} &&
                           type != typeof(IEndpointFeature) &&
                           type.IsAssignableTo(typeof(IEndpointFeature))
            )
            .Select(type => ServiceDescriptor.Transient(typeof(IEndpointFeature), type))
            .ToArray();

        services.TryAddEnumerable(serviceDescriptors);

        return services;
    }
}