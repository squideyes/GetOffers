#region Copyright, Author Details and Related Context   
// Copyright (C) SquidEyes, LLC. - All Rights Reserved
// Unauthorized copying of this file, via any medium is strictly prohibited
// Proprietary and Confidential
// Written by Louis S. Berman <louis@squideyes.com>, 6/20/2016
#endregion  

using System;

namespace GetOffers
{
    public class GenericArgs<T> : EventArgs
    {
        internal GenericArgs(T item)
        {
            Item = item;
        }

        public T Item { get; }
    }
}
