namespace Caravel.Functional
{
    public sealed record Some<T> : Optional<T>
    {
        public T Value { get; }

        internal Some(T value) : base(value, true)
        {
            Value = value;
        }
    }
}