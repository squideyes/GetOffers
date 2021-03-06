﻿#region Copyright, Author Details and Related Context   
// Copyright (C) SquidEyes, LLC. - All Rights Reserved
// Unauthorized copying of this file, via any medium is strictly prohibited
// Proprietary and Confidential
// Written by Louis S. Berman <louis@squideyes.com>, 6/21/2016
#endregion  

using fxcore2;
using System;
using System.Threading;

namespace GetOffers
{
    public class ResponseListener : IO2GResponseListener
    {
        private string requestId;
        private O2GResponse response;
        private EventWaitHandle syncResponseEvent;

        public ResponseListener()
        {
            requestId = string.Empty;
            response = null;
            syncResponseEvent = new EventWaitHandle(false, EventResetMode.AutoReset);
        }

        public void SetRequestID(string requestId)
        {
            response = null;
            this.requestId = requestId;
        }

        public bool WaitEvents()
        {
            return syncResponseEvent.WaitOne(30000);
        }

        public O2GResponse GetResponse() => response;

        public void onRequestCompleted(string sRequestId, O2GResponse response)
        {
            if (requestId.Equals(response.RequestID))
            {
                this.response = response;

                syncResponseEvent.Set();
            }
        }

        public void onRequestFailed(string requestId, string sError)
        {
            if (this.requestId.Equals(requestId))
            {
                Console.WriteLine("Request failed: " + sError);

                syncResponseEvent.Set();
            }
        }

        public void onTablesUpdates(O2GResponse data)
        {
        }
    }
}
