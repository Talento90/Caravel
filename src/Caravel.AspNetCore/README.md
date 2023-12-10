# Caravel.AspNetCore

This package contains reusable middleware, http utilities that every application needs.

* Exception Middleware

```c#
// Handle exceptions according to RFC: https://tools.ietf.org/html/rfc7807
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
```

```json
{
  "code": "book_not_found",
  "title": "Book does not exist",
  "status": 404,
  "detail": "Book 53655b3d-48d5-4ac1-ba73-4318b3b702e8 does not exist",
}
```
