using System;

namespace Caravel.Clock
{
    /// <summary>
    /// DateTime implementation of the IClock interface.
    /// </summary>
    public class DateTimeClock : IClock
    {
        /// <summary>
        /// This method gets the current DateTime in UTC.
        /// </summary>
        /// <returns>DateTime.UtcNow</returns>
        public DateTime NowUtc()
        {
            return DateTime.UtcNow;
        }
    }
}