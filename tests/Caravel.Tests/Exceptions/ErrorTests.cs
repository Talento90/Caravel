using System.Linq;
using System.Reflection;
using Caravel.Exceptions;
using Xunit;

namespace Caravel.Tests.Exceptions
{
    public class ErrorTests
    {
        [Fact]
        public void Ensure_Unique_Error_Codes()
        {
            var errorCodes = typeof(Errors)
                .GetFields(BindingFlags.Public | BindingFlags.Static)
                .Where(f => f.FieldType == typeof(Error))
                .Select(f => f.GetValue(null))
                .Cast<Error>()
                .Select(e => e.Code)
                .ToList();

            Assert.True(errorCodes.Distinct().Count() == errorCodes.Count());
        }
    }
}