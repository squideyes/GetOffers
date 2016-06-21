#region Copyright, Author Details and Related Context   
// Copyright (C) SquidEyes, LLC. - All Rights Reserved
// Unauthorized copying of this file, via any medium is strictly prohibited
// Proprietary and Confidential
// Written by Louis S. Berman <louis@squideyes.com>, 6/20/2016
#endregion  

using System;
using System.Threading;
using fxcore2;

namespace GetOffers
{
    internal class StatusListener : IO2GSessionStatus
    {
        private string sessionId;
        private bool mError;
        private O2GSession session;
        private EventWaitHandle syncSessionEvent = null;

        public event EventHandler<GenericArgs<O2GSessionStatusCode>> OnStatus;

        public StatusListener(O2GSession session, string sessionId)
        {
            this.session = session;
            this.sessionId = sessionId;

            Connected = false;
            mError = false;

            syncSessionEvent = new EventWaitHandle(
                false, EventResetMode.AutoReset);
        }

        public bool Connected { get; private set; }
        public string ErrorMessage { get; private set; }

        public bool Error
        {
            get
            {
                return mError;
            }
        }

        public bool WaitEvents() => syncSessionEvent.WaitOne(30000);

        public void onSessionStatusChanged(O2GSessionStatusCode status)
        {
            OnStatus?.Invoke(this, new GenericArgs<O2GSessionStatusCode>(status));

            switch (status)
            {
                case O2GSessionStatusCode.TradingSessionRequested:
                    session.setTradingSession(sessionId, "");
                    break;
                case O2GSessionStatusCode.Connected:
                    Connected = true;
                    syncSessionEvent.Set();
                    break;
                case O2GSessionStatusCode.Disconnected:
                    Connected = false;
                    syncSessionEvent.Set();
                    break;
            }
        }

        public void onLoginFailed(string errorMessage)
        {
            ErrorMessage = errorMessage;

            mError = true;
        }
    }
}
