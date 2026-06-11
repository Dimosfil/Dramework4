using System;

using Unity.IL2CPP.CompilerServices;


namespace IG.HappyCoder.Plugins.Dramework4.Runtime.Messaging.Signals
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public static partial class DWSignal
    {
        #region ================================ METHODS

        public static void Unsubscribe(string name, Action callback)
        {
            Signal.Unsubscribe(name, callback);
        }

        public static void Unsubscribe<T>(Action callback)
        {
            Signal.Unsubscribe<T>(callback);
        }

        public static void Unsubscribe<T>(Action<T> callback)
        {
            Signal<T>.Unsubscribe("", callback);
        }

        public static void Unsubscribe<T>(string name, Action<T> callback)
        {
            Signal<T>.Unsubscribe(name, callback);
        }

        public static void Unsubscribe<T1, T2>(Action<T1, T2> callback)
        {
            Signal<T1, T2>.Unsubscribe("", callback);
        }

        public static void Unsubscribe<T1, T2>(string name, Action<T1, T2> callback)
        {
            Signal<T1, T2>.Unsubscribe(name, callback);
        }

        public static void Unsubscribe<T1, T2, T3>(Action<T1, T2, T3> callback)
        {
            Signal<T1, T2, T3>.Unsubscribe("", callback);
        }

        public static void Unsubscribe<T1, T2, T3>(string name, Action<T1, T2, T3> callback)
        {
            Signal<T1, T2, T3>.Unsubscribe(name, callback);
        }

        public static void Unsubscribe<T1, T2, T3, T4>(Action<T1, T2, T3, T4> callback)
        {
            Signal<T1, T2, T3, T4>.Unsubscribe("", callback);
        }

        public static void Unsubscribe<T1, T2, T3, T4>(string name, Action<T1, T2, T3, T4> callback)
        {
            Signal<T1, T2, T3, T4>.Unsubscribe(name, callback);
        }

        public static void Unsubscribe<T1, T2, T3, T4, T5>(Action<T1, T2, T3, T4, T5> callback)
        {
            Signal<T1, T2, T3, T4, T5>.Unsubscribe("", callback);
        }

        public static void Unsubscribe<T1, T2, T3, T4, T5>(string name, Action<T1, T2, T3, T4, T5> callback)
        {
            Signal<T1, T2, T3, T4, T5>.Unsubscribe(name, callback);
        }

        public static void Unsubscribe<T1, T2, T3, T4, T5, T6>(Action<T1, T2, T3, T4, T5, T6> callback)
        {
            Signal<T1, T2, T3, T4, T5, T6>.Unsubscribe("", callback);
        }

        public static void Unsubscribe<T1, T2, T3, T4, T5, T6>(string name, Action<T1, T2, T3, T4, T5, T6> callback)
        {
            Signal<T1, T2, T3, T4, T5, T6>.Unsubscribe(name, callback);
        }

        public static void Unsubscribe<T1, T2, T3, T4, T5, T6, T7>(Action<T1, T2, T3, T4, T5, T6, T7> callback)
        {
            Signal<T1, T2, T3, T4, T5, T6, T7>.Unsubscribe("", callback);
        }

        public static void Unsubscribe<T1, T2, T3, T4, T5, T6, T7>(string name, Action<T1, T2, T3, T4, T5, T6, T7> callback)
        {
            Signal<T1, T2, T3, T4, T5, T6, T7>.Unsubscribe(name, callback);
        }

        public static void Unsubscribe<T1, T2, T3, T4, T5, T6, T7, T8>(Action<T1, T2, T3, T4, T5, T6, T7, T8> callback)
        {
            Signal<T1, T2, T3, T4, T5, T6, T7, T8>.Unsubscribe("", callback);
        }

        public static void Unsubscribe<T1, T2, T3, T4, T5, T6, T7, T8>(string name, Action<T1, T2, T3, T4, T5, T6, T7, T8> callback)
        {
            Signal<T1, T2, T3, T4, T5, T6, T7, T8>.Unsubscribe(name, callback);
        }

        public static void Unsubscribe<T1, T2, T3, T4, T5, T6, T7, T8, T9>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9> callback)
        {
            Signal<T1, T2, T3, T4, T5, T6, T7, T8, T9>.Unsubscribe("", callback);
        }

        public static void Unsubscribe<T1, T2, T3, T4, T5, T6, T7, T8, T9>(string name, Action<T1, T2, T3, T4, T5, T6, T7, T8, T9> callback)
        {
            Signal<T1, T2, T3, T4, T5, T6, T7, T8, T9>.Unsubscribe(name, callback);
        }

        public static void Unsubscribe<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> callback)
        {
            Signal<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>.Unsubscribe("", callback);
        }

        public static void Unsubscribe<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(string name, Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> callback)
        {
            Signal<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>.Unsubscribe(name, callback);
        }

        #endregion
    }
}