using System;

namespace Caravel.Clock
{
    /// <summary>
    /// DateTime implementation of the IClock interface.
    /// </summary>
    public class DateTimeUtcClock : IClock
    {
        /// <summary>
        /// This method gets the current DateTime in UTC.
        /// </summary>
        /// <returns>DateTime.UtcNow</returns>
        public DateTime NowUtc()
        {
            return DateTime.UtcNow;
        }

        /// <summary>
        /// This method gets the current DateTimeOffset in UTC.
        /// </summary>
        /// <returns></returns>
        public DateTimeOffset NowOffsetUtc()
        {
            return DateTimeOffset.UtcNow;
        }
    }
}