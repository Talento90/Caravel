using Caravel.Exceptions;
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
            var left = Either.Left<Error, string>(Errors.Validation);

            var result = left.Fold(
                (e) => e.Message,
                (r) => r);

            Assert.Equal(Errors.Validation.Message, result);
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
            var left = Either.Left<Error, string>(Errors.Validation);

            var result = left
                .Map((r) => r.Length)
                .Map((i => i.ToString()))
                .Fold(
                    (l) => l.Message,
                    (r) => r);

            Assert.Equal(Errors.Validation.Message, result);
        }
    }
}