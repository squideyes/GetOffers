#region Copyright, Author Details and Related Context   
// Copyright (C) SquidEyes, LLC. - All Rights Reserved
// Unauthorized copying of this file, via any medium is strictly prohibited
// Proprietary and Confidential
// Written by Louis S. Berman <louis@squideyes.com>, 6/20/2016
#endregion  

using System;
using fxcore2;
using System.Collections.Generic;

namespace GetOffers
{
    internal class TableListener : IO2GTableListener
    {
        private HashSet<string> instruments = new HashSet<string>();

        public event EventHandler<GenericArgs<Offer>> OnOffer;

        public void SetSymbols(List<Symbol> symbols) =>
            symbols.ForEach(s => instruments.Add(s.ToInstrument()));

        public void onAdded(string rowId, O2GRow rowData)
        {
        }

        public void onChanged(string rowId, O2GRow rowData)
        {
            if (rowData.TableType == O2GTableType.Offers)
                HandleOffer((O2GOfferTableRow)rowData);
        }

        public void onDeleted(string rowId, O2GRow rowData)
        {
        }

        public void onStatusChanged(O2GTableStatus status)
        {
        }

        public void SubscribeEvents(O2GTableManager manager)
        {
            var table = (O2GOffersTable)manager.getTable(O2GTableType.Offers);

            table.subscribeUpdate(O2GTableUpdateType.Update, this);
        }

        public void UnsubscribeEvents(O2GTableManager manager)
        {
            var table = (O2GOffersTable)manager.getTable(O2GTableType.Offers);

            table.unsubscribeUpdate(O2GTableUpdateType.Update, this);
        }

        public void HandleOffers(O2GOffersTable offers)
        {
            O2GOfferTableRow offerRow = null;

            var iterator = new O2GTableIterator();

            while (offers.getNextRow(iterator, out offerRow))
                HandleOffer(offerRow);
        }

        public void HandleOffer(O2GOfferTableRow row)
        {
            if (!instruments.Contains(row.Instrument))
                return;

            if (row.isTimeValid && row.isBidValid && row.isAskValid)
            {
                var symbol = row.Instrument.ToSymbol();

                var offer = new Offer()
                {
                    Symbol = symbol,
                    TickOn = new DateTime(row.Time.Ticks, DateTimeKind.Utc).ToEstFromUtc(),
                    BidRate = row.Bid.ToRoundedRate(symbol),
                    AskRate = row.Ask.ToRoundedRate(symbol),
                };

                OnOffer?.Invoke(this, new GenericArgs<Offer>(offer));
            }
        }
    }
}
