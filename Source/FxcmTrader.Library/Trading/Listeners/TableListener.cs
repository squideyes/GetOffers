using System;
using fxcore2;
using System.Collections.Generic;

namespace FxcmTrader.Trading
{
    internal class TableListener : IO2GTableListener
    {
        private HashSet<string> instruments = new HashSet<string>();

        private bool testingMode;

        public event EventHandler<GenericArgs<Offer>> OnOffer;
        public event EventHandler<GenericArgs<Dictionary<string, string>>> OnOfferIds;

        public TableListener(bool testingMode)
        {
            this.testingMode = testingMode;
        }

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
            var offerIds = new Dictionary<string, string>();

            var rows = new List<O2GOfferTableRow>();

            O2GOfferTableRow row = null;

            var iterator = new O2GTableIterator();

            while (offers.getNextRow(iterator, out row))
            {
                rows.Add(row);

                offerIds.Add(row.Instrument, row.OfferID);
            }

            OnOfferIds?.Invoke(this,
                new GenericArgs<Dictionary<string, string>>(offerIds));

            rows.ForEach(r => RaiseOnOfferIfValid(r));
        }

        public void HandleOffer(O2GOfferTableRow row) => RaiseOnOfferIfValid(row);

        private void RaiseOnOfferIfValid(O2GOfferTableRow row)
        {
            if (!instruments.Contains(row.Instrument))
                return;

            if (row.isTimeValid && row.isBidValid && row.isAskValid)
            {
                var tickOn = new DateTime(row.Time.Ticks, DateTimeKind.Utc).ToEstFromUtc();

                if (!testingMode && tickOn.IsTickOn())
                    return;

                var symbol = row.Instrument.ToSymbol();

                var offer = new Offer()
                {
                    Symbol = symbol,
                    TickOn = tickOn,
                    BidRate = row.Bid.ToRoundedRate(symbol),
                    AskRate = row.Ask.ToRoundedRate(symbol),
                };

                OnOffer?.Invoke(this, new GenericArgs<Offer>(offer));
            }
        }
    }
}
