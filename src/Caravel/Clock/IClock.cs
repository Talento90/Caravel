using System;

namespace Caravel.Clock
{
    public interface IClock
    {
        DateTime NowUtc();
    }
}