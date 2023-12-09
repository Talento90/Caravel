namespace Caravel.Functional;

public abstract record Optional<T>
{
    private readonly T _value;
    private readonly bool _hasValue;

    internal Optional(T value, bool hasValue)
    {
        _value = value;
        _hasValue = hasValue;
    }

    public bool TryGetValue(out T value)
    {
        value = (_hasValue ? _value : default)!;
        return _hasValue;
    }
}

public static class Optional
{
    public static Some<T> Some<T>(T value) => new Some<T>(value);

    public static None<T> None<T>() => new None<T>();

    public static Optional<T> From<T>(T value) where T : class
        => value is null
            ? (Optional<T>) None<T>()
            : Some(value);

    public static Optional<T> From<T>(T? value) where T : struct
        => value.HasValue
            ? (Optional<T>) Some(value.Value)
            : None<T>();
}