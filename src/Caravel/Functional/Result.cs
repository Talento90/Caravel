using Caravel.Exceptions;

namespace Caravel.Functional
{
    public static class Result
    {
        public static Either<Error, TValue> Success<TValue>(TValue value)
            => Either.Right<Error, TValue>(value);

        public static Either<Error, TValue> Error<TValue>(Error error)
            => Either.Left<Error, TValue>(error);
    }
}