#region Copyright, Author Details and Related Context   
// Copyright (C) SquidEyes, LLC. - All Rights Reserved
// Unauthorized copying of this file, via any medium is strictly prohibited
// Proprietary and Confidential
// Written by Louis S. Berman <louis@squideyes.com>, 6/20/2016
#endregion  

using fxcore2;
using System;
using System.Threading;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace GetOffers
{
    public class OfferClient
    {
        private const string URL = "http://www.fxcorporate.com/Hosts.jsp";

        private EventWaitHandle cancelEvent =
            new EventWaitHandle(false, EventResetMode.AutoReset);

        private string userName;
        private string password;
        private Connection connection;
        private bool testingMode;

        public event EventHandler<GenericArgs<Status>> OnStatus;
        public event EventHandler<GenericArgs<Offer>> OnOffer;
        public event EventHandler<GenericArgs<Exception>> OnError;
        public event EventHandler OnCancelled;
        public event EventHandler OnDisconnected;

        public OfferClient(string userName, string password, 
            Connection connection, bool testingMode)
        {
            if (string.IsNullOrWhiteSpace(userName))
                throw new ArgumentNullException(nameof(userName));

            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentNullException(nameof(password));

            if (!Enum.IsDefined(typeof(Connection), connection))
                throw new ArgumentOutOfRangeException(nameof(connection));

            this.userName = userName;
            this.password = password;
            this.connection = connection;
            this.testingMode = testingMode;
        }

        public void Start() => Task.Factory.StartNew(() => DoWork());

        private void DoWork()
        {
            O2GSession session = null;
            O2GTableManager tableManager = null;
            StatusListener statusListener = null;
            TableListener tableListener = null;

            bool canUnsubscribeSessionStatus = false;

            try
            {
                session = O2GTransport.createSession();

                session.useTableManager(O2GTableManagerMode.Yes, null);

                var sessionId = Guid.NewGuid().ToString("N");

                statusListener = new StatusListener(session, sessionId);

                statusListener.OnStatus += (s, e) =>
                {
                    var status = e.Item.ToStatus();

                    OnStatus?.Invoke(this, new GenericArgs<Status>(status));

                    if (status == Status.Disconnected)
                        OnDisconnected?.Invoke(this, EventArgs.Empty);
                };

                session.subscribeSessionStatus(statusListener);

                session.login(userName, password, URL, connection.ToString());

                if (statusListener.WaitEvents() && statusListener.Connected)
                {
                    tableListener = new TableListener(testingMode);

                    tableListener.OnOffer += (s, e) =>
                        OnOffer?.Invoke(this, new GenericArgs<Offer>(e.Item));

                    // improve this plus get rid of response listener
                    tableListener.OnOfferIds += (s, e) =>
                    {
                        //var request = GetSetSubscriptionStatusRequest(session, e.Item);

                        //if (request == null)
                        //    throw new Exception("Cannot create request");

                        //var responseListener = new ResponseListener();

                        //session.subscribeResponse(responseListener);

                        //responseListener.SetRequestID(request.RequestID);

                        //session.sendRequest(request);

                        //if (!responseListener.WaitEvents())
                        //    throw new Exception("Response waiting timeout expired");
                    };

                    tableManager = session.getTableManager();

                    var managerStatus = tableManager.getStatus();

                    // max wait
                    while (managerStatus == O2GTableManagerStatus.TablesLoading)
                    {
                        Thread.Sleep(50);

                        managerStatus = tableManager.getStatus();
                    }

                    if (managerStatus == O2GTableManagerStatus.TablesLoadFailed)
                        throw new Exception("TableManager refresh failed!");

                    tableListener.SetSymbols(
                        Enum.GetValues(typeof(Symbol)).Cast<Symbol>().ToList());

                    tableListener.SubscribeEvents(tableManager);

                    O2GOffersTable offers = null;

                    offers = (O2GOffersTable)tableManager.getTable(O2GTableType.Offers);

                    tableListener.HandleOffers(offers);

                    cancelEvent.WaitOne();

                    OnCancelled?.Invoke(this, EventArgs.Empty);
                }
            }
            catch (Exception error)
            {
                OnError?.Invoke(this, new GenericArgs<Exception>(error));
            }
            finally
            {
                Shutdown(session, statusListener, tableListener, tableManager);

                if (canUnsubscribeSessionStatus)
                    session.unsubscribeSessionStatus(statusListener);

                if (session != null)
                    session.Dispose();
            }
        }

        public void Cancel() => cancelEvent.Set();

        private void Shutdown(O2GSession session, StatusListener statusListener,
            TableListener tableListener, O2GTableManager tableManager)
        {
            if (tableListener != null)
                tableListener.UnsubscribeEvents(tableManager);

            if (statusListener != null)
            {
                if (session != null)
                    session.logout();

                statusListener.WaitEvents();
            }
        }

        private O2GRequest GetSetSubscriptionStatusRequest(
            O2GSession session, Dictionary<string, string> offerIds)
        {
            var factory = session.getRequestFactory();

            var mainValueMap = factory.createValueMap();

            mainValueMap.setString(O2GRequestParamsEnum.Command, "SetSubscriptionStatus");

            foreach (var instrument in offerIds.Keys)
            {
                var childValueMap = factory.createValueMap();

                childValueMap.setString(O2GRequestParamsEnum.Command, "SetSubscriptionStatus");

                var status = instrument.IsSymbol() ? "T" : "D";

                childValueMap.setString(O2GRequestParamsEnum.SubscriptionStatus, status);

                childValueMap.setString(O2GRequestParamsEnum.OfferID, offerIds[instrument]);

                mainValueMap.appendChild(childValueMap);
            }

            return factory.createOrderRequest(mainValueMap);
        }
    }
}
