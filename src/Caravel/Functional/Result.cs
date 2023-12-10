using Caravel.Errors;

namespace Caravel.Functional;

public class Result
{
    public Error Error { get; protected init; }
    public bool IsSuccess => Error.Type == ErrorType.None;

    protected Result()
    {
        Error = Error.None;
    }

    private Result(Error error)
    {
        Error = error;
    }

    public static Result Success() => new();
    public static Result Failure(Error error) => new(error);
    public static implicit operator Result(Error error) => Failure(error);
}

public sealed class Result<T> : Result
{
    private readonly T _data;

    public T Data
    {
        get
        {
            ArgumentNullException.ThrowIfNull(_data);
            return _data;
        }
    }

    private Result(T data)
    {
        Error = Error.None;
        _data = data;
    }

    private Result(Error error)
    {
        Error = error;
        _data = default!;
    }

    public static Result<T> Success(T value) => new(value);
    public new static Result<T> Failure(Error error) => new(error);

    public static implicit operator Result<T>(Error error) => new(error);
    public static implicit operator T(Result<T> result) => result.Data;
}

public static class ResultExtensions
{
    public static TOut Map<T, TOut>(
        this Result<T> result,
        Func<T, TOut> success,
        Func<Error, TOut> failure
    )
    {
        return result.IsSuccess ? success(result.Data) : failure(result.Error);
    }

    public static T GetOrDefault<T>(this Result<T> result, T defaultValue)
    {
        return result.IsSuccess ? result.Data : defaultValue;
    }
}