using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Caravel.AspNetCore.OpenApi;

public class OpenApiSchemas
{
    public static void RegisterApiKeySchema(SwaggerGenOptions options)
    {
        options.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme()
        {
            Description = "The API KEY to access the API.",
            Type = SecuritySchemeType.ApiKey,
            Name = "X-API-KEY",
            In = ParameterLocation.Header,
            Scheme = "ApiKeyScheme",
        });

        var scheme = new OpenApiSecurityScheme()
        {
            In = ParameterLocation.Header,
            Reference = new OpenApiReference()
            {
                Type = ReferenceType.SecurityScheme,
                Id = "ApiKey"
            }
        };

        var requirement = new OpenApiSecurityRequirement()
        {
            {scheme, []}
        };

        options.AddSecurityRequirement(requirement);
    }
}