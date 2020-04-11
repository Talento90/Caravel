using System;
using Caravel.Clock;
using Xunit;

namespace Caravel.Tests.Clock
{
    public class ClockTests
    {
        [Fact]
        public void Should_Return_Utc_Kind()
        {
            // Arrange
            IClock clock = new DateTimeClock();
            
            //Act
            var nowUtc = clock.NowUtc();
            
            //Assert
            Assert.Equal(DateTimeKind.Utc, nowUtc.Kind);
        }
    }
}