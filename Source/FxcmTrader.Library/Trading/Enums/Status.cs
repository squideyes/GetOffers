using fxcore2;

namespace FxcmTrader.Trading
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
