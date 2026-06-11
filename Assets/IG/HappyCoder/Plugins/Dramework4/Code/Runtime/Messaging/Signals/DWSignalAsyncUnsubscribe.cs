using System;


namespace IG.HappyCoder.Plugins.Dramework4.Runtime.Messaging.Signals
{
    public static partial class DWSignalAsync
    {
        #region ================================ METHODS

        public static void Unsubscribe(string name, Action callback)
        {
            SignalAsync.Unsubscribe(name, callback);
        }

        public static void Unsubscribe<T>(Action callback)
        {
            SignalAsync.Unsubscribe<T>(callback);
        }

        public static void Unsubscribe<T>(Action<T> callback)
        {
            SignalAsync<T>.Unsubscribe("", callback);
        }

        public static void Unsubscribe<T>(string name, Action<T> callback)
        {
            SignalAsync<T>.Unsubscribe(name, callback);
        }

        #endregion
    }
}