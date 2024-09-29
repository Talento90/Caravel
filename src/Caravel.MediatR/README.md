# Caravel.MediatR

This package contains reusable MediatR behaviours.

### Logging Pipeline Behaviour

Creates an entry Log for the start and failure of each request in the pipeline. It will log the error in case the request fails.  

```c#
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
    cfg.AddOpenBehavior(typeof(LoggingPipelineBehaviour<,>));
});
```

### Validation Pipeline Behaviour

Validates requests using FluentValidation. This behaviour detects via reflection if the Response is using the `Result` pattern to avoid throwing unnecessary `ValidationException`, if not then throws it.

```c#
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
    cfg.AddOpenBehavior(typeof(ValidationPipelineBehaviour<,>));
});
```

```json
{
  "code": "invalid_fields",
  "title": "Invalid Fields",
  "status": 400,
  "errors": {
    "field1": ["error_1", "error_2"]
  }
}
```

### Idempotent Pipeline Behaviour

Ensures all requests implementing interface `IIdempotentRequest` are idempotent.

```c#
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
    cfg.AddOpenBehavior(typeof(IdempotentPipelineBehaviour<,>));
});
```

```json
{
  "code": "duplicate_request",
  "title": "Duplicate request",
  "status": 204,
  "detail": "Duplicate request with Idempotent Key 53655b3d-48d5-4ac1-ba73-4318b3b702e8."
}
```

### Caching Pipeline Behaviour

Caches responses where the request implements `ICachedQuery`.

```c#
// Requires implementing the caching service.
builder.Services.AddSingleton<ICacheService, CacheService>();
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
    cfg.AddOpenBehavior(typeof(CachingPipelineBehavior<,>));
});
```

```json
{
  "code": "duplicate_request",
  "title": "Duplicate request",
  "status": 204,
  "detail": "Duplicate request with Idempotent Key 53655b3d-48d5-4ac1-ba73-4318b3b702e8."
}
```