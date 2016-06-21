#region Copyright, Author Details and Related Context   
// Copyright (C) SquidEyes, LLC. - All Rights Reserved
// Unauthorized copying of this file, via any medium is strictly prohibited
// Proprietary and Confidential
// Written by Louis S. Berman <louis@squideyes.com>, 6/20/2016
#endregion  

using fxcore2;

namespace GetOffers
{
    public enum Status
    {
        Connected = O2GSessionStatusCode.Connected,
        Connecting = O2GSessionStatusCode.Connecting,
        Disconnected = O2GSessionStatusCode.Disconnected,
        Disconnecting = O2GSessionStatusCode.Disconnecting,
        PriceSessionReconnecting = O2GSessionStatusCode.PriceSessionReconnecting,
        Reconnecting = O2GSessionStatusCode.Reconnecting,
        SessionLost = O2GSessionStatusCode.SessionLost,
        TradingSessionRequested = O2GSessionStatusCode.TradingSessionRequested,
        Unknown = O2GSessionStatusCode.Unknown
    }
}
