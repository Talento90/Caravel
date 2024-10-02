namespace Caravel.AspNetCore.Http;

public class UnauthorizedResult : IResult, IStatusCodeHttpResult
{
    private object _body;

    public UnauthorizedResult(object body)
    {
        _body = body;
    }

    public async Task ExecuteAsync(HttpContext httpContext)
    {
        ArgumentNullException.ThrowIfNull(httpContext);
        httpContext.Response.StatusCode = StatusCode.GetValueOrDefault();

        if (_body is string stringBody)
        {
            await httpContext.Response.WriteAsync(stringBody);
            return;
        }
        
        await httpContext.Response.WriteAsJsonAsync(_body);
    }

    public int? StatusCode => StatusCodes.Status401Unauthorized;
}