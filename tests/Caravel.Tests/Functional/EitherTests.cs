using Caravel.Errors;
using Caravel.Functional;
using Xunit;

namespace Caravel.Tests.Functional
{
    public class EitherTests
    {
        [Fact]
        public void Fold_Should_Return_Right_Value()
        {
            var right = Either.Right<Error, string>("Success");

            var result = right.Fold(
                (e) => e.Message,
                (r) => r);

            Assert.Equal("Success", result);
        }

        [Fact]
        public void Fold_Should_Return_Left_Value()
        {
            var left = Either.Left<string, string>("error");

            var result = left.Fold(
                (e) => e,
                (r) => r);

            Assert.Equal("error", result);
        }

        [Fact]
        public void Map_Should_Return_Right_Value()
        {
            var right = Either.Right<Error, string>("Success");

            var result = right
                    .Map((r) => r.Length)
                    .Map((i => i.ToString()))
                    .Fold(
                        (l) => l.Message,
                        (r) => r);

            Assert.Equal("7", result);
        }
        
        [Fact]
        public void Map_Should_Return_Left_Value()
        {
            var left = Either.Left<string, string>("error");

            var result = left
                .Map((r) => r.Length)
                .Map((i => i.ToString()))
                .Fold(
                    (l) => l,
                    (r) => r);

            Assert.Equal("error", result);
        }
        
        [Fact]
        public void Success_Should_Return_Right_Value()
        {
            var right = Either.Success<Error, string>("Success");

            var result = right
                .Map((r) => r.Length)
                .Map((i => i.ToString()))
                .Fold(
                    (l) => l.Message,
                    (r) => r);

            Assert.Equal("7", result);
        }
        
        [Fact]
        public void Failure_Should_Return_Left_Value()
        {
            var failure = Either.Failure<Error, string>(
                new Error("error_code", ErrorType.Permission, "error")
                );

            var result = failure
                .Map((r) => r.Length)
                .Map((i => i.ToString()))
                .Fold(
                    (l) => l.Message,
                    (r) => r);

            Assert.Equal("error", result);
        }
    }
}