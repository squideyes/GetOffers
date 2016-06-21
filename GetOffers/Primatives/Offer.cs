#region Copyright, Author Details and Related Context   
// Copyright (C) SquidEyes, LLC. - All Rights Reserved
// Unauthorized copying of this file, via any medium is strictly prohibited
// Proprietary and Confidential
// Written by Louis S. Berman <louis@squideyes.com>, 6/20/2016
#endregion  

using System;
using System.Text;

namespace GetOffers
{
    public class Offer
    {
        public Symbol Symbol { get; internal set; }
        public DateTime TickOn { get; internal set; }
        public double BidRate { get; internal set; }
        public double AskRate { get; internal set; }

        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.Append(Symbol);
            sb.Append(',');
            sb.Append(TickOn.ToTickOnString());
            sb.Append(',');
            sb.Append(BidRate.ToRateString(Symbol));
            sb.Append(',');
            sb.Append(AskRate.ToRateString(Symbol));

            return sb.ToString();
        }
    }
}
