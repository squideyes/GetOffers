using System;
using System.Collections.Generic;
using System.Linq;

namespace FxcmTrader.Trading
{
    public static class SymbolExtenders
    {
        private static HashSet<string> instruments = 
            new HashSet<string>(Enum.GetValues(typeof(Symbol))
                .Cast<Symbol>().Select(s => s.ToInstrument()));

        public static string ToInstrument(this Symbol symbol)
        {
            var s = symbol.ToString();

            return s.Substring(0, 3) + "/" + s.Substring(3);
        }

        public static bool IsSymbol(this string instrument) => 
            instruments.Contains(instrument);
    }
}
