# Caravel.MediatR

This package contains reusable MediatR behaviours.

### Logging Behaviour

Creates an entry Log for the start and failure of each request in the pipeline. It will log the error in case the request fails.  

```c#
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
    cfg.AddOpenBehavior(typeof(LoggingRequestBehaviour<,>));
});
```

### Validation and ResultValidation Behaviour

Validates requests using FluentValidation. 
It is recommended to use `ResultValidationRequestBehaviour` when using the `Result` pattern to avoid throwing unnecessary `ValidationException`.

```c#
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
    cfg.AddOpenBehavior(typeof(ValidationRequestBehaviour<,>));
    cfg.AddOpenBehavior(typeof(ResultValidationRequestBehaviour<,>));
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

### Idempotent Behaviour

Ensures all requests implementing interface `IIdempotentRequest` are idempotent.

```c#
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
    cfg.AddOpenBehavior(typeof(IdempotentRequestBehaviour<,>));
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