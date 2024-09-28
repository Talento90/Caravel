namespace Caravel.AspNetCore.Endpoint;

public interface IEndpointFeature
{
    void AddEndpoint(IEndpointRouteBuilder endpointBuilder);
}

