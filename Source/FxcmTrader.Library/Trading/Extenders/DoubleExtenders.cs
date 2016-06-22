using System;

namespace FxcmTrader.Trading
{
    public static class DoubleExtenders
    {
        public static double ToRoundedRate(this double value, Symbol symbol) =>
            Math.Round(value, symbol == Symbol.USDJPY ? 3 : 5);

        public static string ToRateString(this double value, Symbol symbol) =>
            value.ToString(symbol == Symbol.USDJPY ? "N3" : "N5");
    }
}
