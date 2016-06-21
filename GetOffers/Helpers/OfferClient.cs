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

namespace GetOffers
{
    public class OfferClient
    {
        private const string URL = "http://www.fxcorporate.com/Hosts.jsp";

        private EventWaitHandle cancelEvent =
            new EventWaitHandle(false, EventResetMode.AutoReset);

        private string userName;
        private string password;

        public event EventHandler<GenericArgs<Status>> OnStatus;
        public event EventHandler<GenericArgs<Offer>> OnOffer;
        public event EventHandler<GenericArgs<Exception>> OnError;
        public event EventHandler OnCancelled;
        public event EventHandler OnDisconnected;

        public OfferClient(string userName, string password)
        {
            if (string.IsNullOrWhiteSpace(userName))
                throw new ArgumentNullException(nameof(userName));

            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentNullException(nameof(password));

            this.userName = userName;
            this.password = password;
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

                session.login(Properties.Settings.Default.UserName,
                    Properties.Settings.Default.Password, URL, "Demo");

                if (statusListener.WaitEvents() && statusListener.Connected)
                {
                    tableListener = new TableListener();

                    tableListener.OnOffer += (s, e) =>
                        OnOffer?.Invoke(this, new GenericArgs<Offer>(e.Item));

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
    }
}
