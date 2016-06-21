#region Copyright, Author Details and Related Context   
// Copyright (C) SquidEyes, LLC. - All Rights Reserved
// Unauthorized copying of this file, via any medium is strictly prohibited
// Proprietary and Confidential
// Written by Louis S. Berman <louis@squideyes.com>, 6/20/2016
#endregion  

using System;

namespace GetOffers
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
