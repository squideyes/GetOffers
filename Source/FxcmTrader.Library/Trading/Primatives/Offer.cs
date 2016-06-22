using System;
using System.Text;

namespace FxcmTrader.Trading
{
    public class Offer
    {
        public Symbol Symbol { get; internal set; }
        public DateTime TickOn { get; internal set; }
        public double BidRate { get; internal set; }
        public double AskRate { get; internal set; }

        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.Append(Symbol);
            sb.Append(',');
            sb.Append(TickOn.ToTickOnString());
            sb.Append(',');
            sb.Append(BidRate.ToRateString(Symbol));
            sb.Append(',');
            sb.Append(AskRate.ToRateString(Symbol));

            return sb.ToString();
        }
    }
}
