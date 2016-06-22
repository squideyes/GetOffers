using fxcore2;

namespace FxcmTrader.Trading
{
    public static class O2GSessionStatusCodeExtenders
    {
        public static Status ToStatus(this O2GSessionStatusCode value) =>
            (Status)(int)value;
    }
}
