# Caravel ![](https://github.com/Talento90/Caravel/workflows/Publish/badge.svg)

<img src="https://raw.githubusercontent.com/Talento90/Caravel/master/assets/logo.svg" width="200">

Caravel is a SDK to help developers building .NET applications.

## Features

This project is split in 2 different packages:

### Caravel

This package does not have any external dependency and it brings all basic utilities that every application should handle such as application context or exceptions.

* Application context (UserId, TraceId, TenantId)

```c#
//Inject IAppContextAccessor on constructor.
IAppContextAccessor contextAccessor = ...

//Get the application context.
AppContext context = contextAccessor.Context;

//Get UserId of the current user.
context.UserId;

//Get TenantId of the current tenant. It is useful when dealing with multitenant applications.
context.TenantId;

//Get TraceId of the current execution.
context.TraceId;
```
* Exceptions and Errors (NotFound, Validation, Permission, etc...)

```c#
var DatabaseError = new Error(10001, "Cannot connect with Database", Severity.High);

var exception = new CaravelException(DatabaseError, innerException);
```

* DateTime Clock

```c#
IClock clock = new DateTimeClock();

var nowUtc = clock.NowUtc();
```
* HttpClient Extension Methods

```c#
HttpClient client = new HttpClient();

client.PostJsonAsync("api/v1/books", createBook, cancellationToken);

```

* Json Converters

```c#
var json = JsonSerializer.Serialize(obj, JsonSerializerOptions.CamelCase());
```

* Functional

Optional - Avoid Nulls using Optional Types
```c#
Optional<Product> result = await service.GetProductAsync(id);

return result switch {
            Some<Product> p => Ok(p),
            _ => NotFound()
        };
```

Either - Improve your error handling by using Either<Error,Success>

```c#
Either.Right<Error, Product> result = await service.GetProductAsync(id);

return result.Fold(
    (e) => new HttpError(e.Message),
    (product) => Ok(product));
```


### Caravel.AspNetCore

This package contains reusable middleware, http utilities that every application needs.

* ValidateModelFilter

```c#
.AddMvcOptions(opt =>
{
    opt.Filters.Add(new ValidateModelFilter());
})
```
* Application version Middleware

```c#
app.UseAppVersion("/api/version");
```
* Logging Middleware

```c#
app.UseMiddleware<LoggingMiddleware>(Options.Create(new LoggingOptions
{
    EnableLogBody = true
}));

```
* Exception Middleware

```c#
// Handle exceptions according to RFC: https://tools.ietf.org/html/rfc7807
app.UseMiddleware<ExceptionMiddleware>();
```

```json
{
  "traceId": "30b860d8-03cd-440e-9653-d5f0d090d86a",
  "code": 30001,
  "title": "Book does not exist",
  "status": 404,
  "detail": "Book 53655b3d-48d5-4ac1-ba73-4318b3b702e8 does not exist",
  "instance": "/api/v1/books/53655b3d-48d5-4ac1-ba73-4318b3b702e8"
}
```

* TraceId/CorrelationId Middleware

```c#
app.UseMiddleware<TraceIdMiddleware>();
```


## Projects using Caravel

* [Caravel Template](https://github.com/Talento90/Caravel-Template) - .NET template that generates a functional web api using the *best practices*.
* [.NET Careers](https://dotnet.careers) - Connecting .NET engineers with amazing companies and help them to find their next challenge.


## Credits

Logo made by <a href="https://www.flaticon.com/authors/freepik" title="Freepik">Freepik</a> from <a href="https://www.flaticon.com/" title="Flaticon"> www.flaticon.com</a>
