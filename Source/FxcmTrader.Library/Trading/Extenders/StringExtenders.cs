using System;

namespace FxcmTrader.Trading
{
    public static class StringExtenders
    {
        public static T ToEnum<T>(this string value) =>
            (T)Enum.Parse(typeof(T), value, true);

        public static Symbol ToSymbol(this string value)
        {
            return (Symbol)Enum.Parse(typeof(Symbol),
                value.Substring(0, 3) + value.Substring(4), true);
        }
    }
}
