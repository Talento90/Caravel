using System;

namespace Caravel.Clock
{
    public class DateTimeClock : IClock
    {
        public DateTime NowUtc()
        {
            return DateTime.UtcNow;
        }
    }
}