namespace Caravel.Functional
{
    public sealed class None<T> : Optional<T>
    {
        internal None() : base(default!, false)
        {
        }
    }
}