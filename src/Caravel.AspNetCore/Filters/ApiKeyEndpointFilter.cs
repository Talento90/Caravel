using Caravel.AspNetCore.Http;
using Caravel.Errors;
using Microsoft.Extensions.Options;

namespace Caravel.AspNetCore.Filters;

public class ApiKeyOptions
{
    public string ApiKey { get; set; } = null!;
}

public class ApiKeyEndpointFilter(IOptions<ApiKeyOptions> options) : IEndpointFilter
{
    private const string ApiKeyHeaderName = "X-API-KEY";
    private readonly string _apiKeyValue = options.Value.ApiKey ?? throw new ArgumentNullException(nameof(options.Value.ApiKey));
    
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        if (!context.HttpContext.Request.Headers.TryGetValue(ApiKeyHeaderName, out var extractedApiKey))
        {
            var missingHeader = new ApiProblemDetails(
                Error.Unauthorized("api_key_missing", $"Provide {ApiKeyHeaderName} header.")
            );

            return new UnauthorizedResult(missingHeader);
        }

        if (!_apiKeyValue.Equals(extractedApiKey))
        {
            var invalidApiKey = new ApiProblemDetails(
                Error.Unauthorized("api_key_invalid", "API Key is not valid.")
            );          
            
            return new UnauthorizedResult(invalidApiKey);
        }

        return await next(context);
    }
}