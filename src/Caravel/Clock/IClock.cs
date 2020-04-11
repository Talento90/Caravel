using System;

namespace Caravel.Clock
{
    /// <summary>
    /// Provides a virtual clock implementation to get the current DateTime.
    /// </summary>
    public interface IClock
    {
        /// <summary>
        /// This method gets the current DateTime in UTC.
        /// </summary>
        /// <returns>DateTime.UtcNow</returns>
        DateTime NowUtc();
    }
}