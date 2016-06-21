#region Copyright, Author Details and Related Context   
// Copyright (C) SquidEyes, LLC. - All Rights Reserved
// Unauthorized copying of this file, via any medium is strictly prohibited
// Proprietary and Confidential
// Written by Louis S. Berman <louis@squideyes.com>, 6/20/2016
#endregion  

using System;

namespace GetOffers
{
    public static class WellKnown
    {
        public static readonly TimeSpan SessionStart = TimeSpan.FromHours(2);
        public static readonly TimeSpan SessionEnd = new TimeSpan(0, 15, 59, 59, 999);
    }
}