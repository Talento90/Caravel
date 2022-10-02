using Caravel.Errors;

namespace Caravel.Functional
{
    public abstract record Either<TLeft, TRight>
    {
        private Either()
        {
        }

        public sealed record Left : Either<TLeft, TRight>
        {
            public Left(TLeft value)
            {
                Value = value;
            }

            public TLeft Value { get; }
        }

        public sealed record Right : Either<TLeft, TRight>
        {
            public Right(TRight value)
            {
                Value = value;
            }

            public TRight Value { get; }
        }
    }
    
    public static class Either
    {
        public static Either<TLeft, TRight> Left<TLeft, TRight>(TLeft value)
            => new Either<TLeft, TRight>.Left(value);

        public static Either<TLeft, TRight> Right<TLeft, TRight>(TRight value)
            => new Either<TLeft, TRight>.Right(value);
        
        public static Either<Error, TValue> Success<TValue>(TValue value)
            => Right<Error, TValue>(value);
        public static Either<Error, TValue> Error<TValue>(Error error)
            => Left<Error, TValue>(error);
    }
}