using Caravel.Errors;

namespace Caravel.Result;

public sealed record Result
{
    public required ResultType Type;
    
    private readonly Error<TError>? _error;

    public Error<TError> Error
    {
        get
        {
            ArgumentNullException.ThrowIfNull(_error);
            return _error!;
        }
    }

    public bool HasError => _error is not null;

    private Result()
    {
    }

    private Result(Error<TError> error)
    {
        _error = error;
    }

    public static Result<TError> Success() => new();
    public static Result<TError> Failure(Error<TError> error) => new(error);

    public static implicit operator Result<TError>(Error<TError> error) => Failure(error);
}

public sealed record Result<T, TError>
{
    private readonly T _data;
    private readonly Error<TError>? _error;

    public T Data
    {
        get
        {
            ArgumentNullException.ThrowIfNull(_data);
            return _data;
        }
    }

    public Error<TError> Error
    {
        get
        {
            ArgumentNullException.ThrowIfNull(_error);
            return _error;
        }
    }

    public bool HasError => _error is not null;

    private Result(T data)
    {
        _data = data;
    }

    private Result(Error<TError> error)
    {
        _error = error;
        _data = default!;
    }

    public static Result<T, TError> Success(T value) => new(value);
    public static Result<T, TError> Failure(Error<TError> error) => new(error);

    public static implicit operator Result<T, TError>(Error<TError> error) => new(error);
}

public static class ResultExtensions
{
    public static TOut Map<T, TError, TOut>(
        this Result<T, TError> result,
        Func<T, TOut> success,
        Func<Error<TError>, TOut> failure
    )
    {
        return result.HasError ? failure(result.Error) : success(result.Data);
    }
    
    public static T GetOrDefault<T, TError>(this Result<T, TError> result, T defaultValue)
    {
        return result.HasError ? defaultValue : result.Data;
    }
}