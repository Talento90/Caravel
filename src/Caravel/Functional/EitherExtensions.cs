namespace Caravel.Functional;

public static class EitherExtensions
{
    public static TOut Fold<TLeftIn, TRightIn, TOut>(
        this Either<TLeftIn, TRightIn> result,
        Func<TLeftIn, TOut> left,
        Func<TRightIn, TOut> right)
    {
        return result switch
        {
            Either<TLeftIn, TRightIn>.Left error => left(error.Value),
            Either<TLeftIn, TRightIn>.Right success => right(success.Value),
            _ => throw new Exception(nameof(result))
        };
    }

    public static async Task<TOut> FoldAsync<TLeftIn, TRightIn, TOut>(
        this Either<TLeftIn, TRightIn> result,
        Func<TLeftIn, Task<TOut>> left,
        Func<TRightIn, Task<TOut>> right)
    {
        return result switch
        {
            Either<TLeftIn, TRightIn>.Left error => await left(error.Value),
            Either<TLeftIn, TRightIn>.Right success => await right(success.Value),
            _ => throw new Exception(nameof(result))
        };
    }

    public static Either<TLeft, TRightOut> Map<TLeft, TRightIn, TRightOut>(
        this Either<TLeft, TRightIn> result,
        Func<TRightIn, TRightOut> right)
    {
        return result switch
        {
            Either<TLeft, TRightIn>.Left error => Either.Left<TLeft, TRightOut>(error.Value),
            Either<TLeft, TRightIn>.Right success => Either.Right<TLeft, TRightOut>(right(success.Value)),
            _ => throw new Exception(nameof(result))
        };
    }

    public static async Task<Either<TLeft, TRightOut>> MapAsync<TLeft, TRightIn, TRightOut>(
        this Either<TLeft, TRightIn> result,
        Func<TRightIn, Task<TRightOut>> right)
    {
        return result switch
        {
            Either<TLeft, TRightIn>.Left error => Either.Left<TLeft, TRightOut>(error.Value),
            Either<TLeft, TRightIn>.Right success => Either.Right<TLeft, TRightOut>(await right(success.Value)),
            _ => throw new Exception(nameof(result))
        };
    }
}