#region Copyright, Author Details and Related Context   
// Copyright (C) SquidEyes, LLC. - All Rights Reserved
// Unauthorized copying of this file, via any medium is strictly prohibited
// Proprietary and Confidential
// Written by Louis S. Berman <louis@squideyes.com>, 6/20/2016
#endregion  

using System;

namespace GetOffers
{
    public static class DoubleExtenders
    {
        public static double ToRoundedRate(this double value, Symbol symbol) =>
            Math.Round(value, symbol == Symbol.USDJPY ? 3 : 5);

        public static string ToRateString(this double value, Symbol symbol) =>
            value.ToString(symbol == Symbol.USDJPY ? "N3" : "N5");
    }
}
