# Caravel.AspNetCore

This package contains reusable middleware, http utilities that every application needs.

* Exception Middleware

```c#
// Handle exceptions according to RFC: https://tools.ietf.org/html/rfc7807
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

// Map the middleware
_application.UseExceptionHandler();
```

```json
{
  "code": "book_not_found",
  "title": "Book does not exist",
  "status": 404,
  "detail": "Book 53655b3d-48d5-4ac1-ba73-4318b3b702e8 does not exist",
}
```

* Endpoint Features

Add all endpoints features that implement `IEndpointFeature` interface.
```c#
builder.Services.AddEndpointFeatures(Assembly.GetExecutingAssembly());
```
Map feature endpoints.
```csharp
ApiVersionSet apiVersionSet = app.NewApiVersionSet()
.HasApiVersion(new ApiVersion(1))
.ReportApiVersions()
.Build();

RouteGroupBuilder versionedGroup = app
.MapGroup("api/v{version:apiVersion}")
.WithApiVersionSet(apiVersionSet);

app.MapEndpointFeatures(versionedGroup);
```

* User Context Implementation using `HttpContextAccessor`

```c#
// Requirement since it's injected in the UserContext implementation
services.AddHttpContextAccessor();
services.AddScoped<IUserContext, UserContext>();
```

```c#
// Inject IUserContext in your services

public MyService(IUserContext userContext) 
{
    var userId = userContext.UserId();
}
```

