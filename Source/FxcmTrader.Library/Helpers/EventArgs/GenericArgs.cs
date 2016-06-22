using System;

namespace FxcmTrader.Helpers
{
    public class GenericArgs<T> : EventArgs
    {
        internal GenericArgs(T item)
        {
            Item = item;
        }

        public T Item { get; }
    }
}
