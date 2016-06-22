using System;

namespace FxcmTrader.Trading
{
    public static class WellKnown
    {
        public static readonly TimeSpan SessionStart = TimeSpan.FromHours(2);
        public static readonly TimeSpan SessionEnd = new TimeSpan(0, 15, 59, 59, 999);
    }
}