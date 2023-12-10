# Caravel ![](https://github.com/Talento90/Caravel/workflows/Publish/badge.svg)

This package does not have any external dependency and it brings all basic utilities that every application should handle such as application context or exceptions.

* Application context (User)

```c#
//Inject IAppContextAccessor on constructor.
IApplicationContextAccessor contextAccessor = ...

//Get the application context.
ApplicationContext context = contextAccessor.Context;

//Get User Id of the current user.
ApplicationContext.User.Id();

//Get TenantId of the current tenant. It is useful when dealing with multitenant applications.
ApplicationContext.TenantId();
```


* Handling Errors

```c#
var customRawError = new Error("invalid_password", ErrorType.Validation, "Password does not match.", "Password does not match with the username.", Severity.Low);

# recommendation: use the static method to generate errors.

public static Error BookNotFoundError(Guid id) => Error.NotFound("book_not_found", $"Book {id} does not exist.");
```

* Handling Exceptions

```c#
var databaseError = new Error("database_connection", ErrorType.Internal, "Cannot create database connection.", "The database is down.", Severity.Critical);

var exception = new CaravelException(databaseError, innerException);
```

* DateTime Clock

```c#
IClock clock = new DateTimeClock();

var nowUtc = clock.NowUtc();
```


* Json Serializers

```c#
    var model = new Dto("Caravel");
    var json = await model.SerializeAsync(JsonSerializerConfigurations.CamelCaseSerializer());
```

* Functional

Result - Use Results instead of throwing exceptions
```c#
var result = Result<string>.Success("Success Value")
    .Map((success) => success.Length,
        (error) => error.Message.Length
    );

    var result = Result<string>.Failure(new Error("code_error", ErrorType.Validation, "Failure Value"))
        .Map((success) => success.Length,
            (error) => error.Message.Length
        );
```

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
