#region Copyright, Author Details and Related Context   
// Copyright (C) SquidEyes, LLC. - All Rights Reserved
// Unauthorized copying of this file, via any medium is strictly prohibited
// Proprietary and Confidential
// Written by Louis S. Berman <louis@squideyes.com>, 6/20/2016
#endregion  

using fxcore2;

namespace GetOffers
{
    public static class O2GSessionStatusCodeExtenders
    {
        public static Status ToStatus(this O2GSessionStatusCode value) =>
            (Status)(int)value;
    }
}
