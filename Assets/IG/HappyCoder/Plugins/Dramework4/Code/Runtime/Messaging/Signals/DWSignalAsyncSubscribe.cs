using System;


namespace IG.HappyCoder.Plugins.Dramework4.Runtime.Messaging.Signals
{
    public static partial class DWSignalAsync
    {
        #region ================================ METHODS

        public static void Subscribe(string name, Action callback, int order = 0)
        {
            SignalAsync.Subscribe(name, callback, order);
        }

        public static void Subscribe<T>(Action callback, int order = 0)
        {
            SignalAsync.Subscribe<T>(callback, order);
        }

        public static void Subscribe<T>(Action<T> callback, int order = 0)
        {
            SignalAsync<T>.Subscribe("", callback, order);
        }

        public static void Subscribe<T>(string name, Action<T> callback, int order = 0)
        {
            SignalAsync<T>.Subscribe(name, callback, order);
        }

        #endregion
    }
}