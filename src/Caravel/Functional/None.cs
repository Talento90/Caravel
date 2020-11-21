namespace Caravel.Functional
{
    public sealed record None<T> : Optional<T>
    {
        internal None() : base(default!, false)
        {
        }
    }
}