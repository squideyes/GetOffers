#region Copyright, Author Details and Related Context   
// Copyright (C) SquidEyes, LLC. - All Rights Reserved
// Unauthorized copying of this file, via any medium is strictly prohibited
// Proprietary and Confidential
// Written by Louis S. Berman <louis@squideyes.com>, 6/20/2016
#endregion  

using System;
using System.Collections.Generic;
using System.Linq;

namespace GetOffers
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
