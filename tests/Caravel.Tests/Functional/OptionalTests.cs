using Caravel.Functional;
using Xunit;

namespace Caravel.Tests.Functional
{
    public class OptionalTests
    {
        [Fact]
        public void Some_Should_Return_Value()
        {
            var some = Optional.Some("Text");

            Assert.True(some.TryGetValue(out var value));
            Assert.Equal("Text", value);
            Assert.Equal("Text", some.Value);
        }

        [Fact]
        public void None_Should_Return_Default_Value()
        {
            var none = Optional.None<string>();

            Assert.False(none.TryGetValue(out var value));
            Assert.Null(value);
        }

        [Fact]
        public void From_Should_Return_Some_ReferenceType()
        {
            var some = Optional.From("value");

            Assert.True(some.TryGetValue(out var value));
            Assert.Equal("value", value);
        }

        [Fact]
        public void From_Should_Return_None_ReferenceType()
        {
            var none = Optional.From<string>(default!);

            Assert.False(none.TryGetValue(out var value));
            Assert.Null(value);
        }

        [Fact]
        public void From_Should_Return_Some_ValueType()
        {
            var some = Optional.From<int>(3);

            Assert.True(some.TryGetValue(out var value));
            Assert.Equal(3, value);
        }

        [Fact]
        public void From_Should_Return_None_ValueType()
        {
            var none = Optional.From<int>(default!);

            Assert.False(none.TryGetValue(out var value));
            Assert.Equal(0, value);
        }

        [Fact]
        public void Should_Return_Some()
        {
            var optional = Optional.From("Text");

            var value = optional switch
            {
                Some<string> e => e.Value,
                _ => string.Empty
            };

            Assert.Equal("Text", value);
        }
        
        [Fact]
        public void Should_Return_None()
        {
            var optional = Optional.From<int>(null);

            var value = optional switch
            {
                Some<int> e => e.Value,
                _ => 0
            };

            Assert.Equal(0, value);
        }
    }
}