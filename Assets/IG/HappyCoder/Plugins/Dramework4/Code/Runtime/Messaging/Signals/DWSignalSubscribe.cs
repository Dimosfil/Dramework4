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

        public static void Subscribe(string name, Action callback, int order = 0)
        {
            Signal.Subscribe(name, callback, order);
        }

        public static void Subscribe<T>(Action callback, int order = 0)
        {
            Signal.Subscribe<T>(callback, order);
        }

        public static void Subscribe<T>(Action<T> callback, int order = 0)
        {
            Signal<T>.Subscribe("", callback, order);
        }

        public static void Subscribe<T>(string name, Action<T> callback, int order = 0)
        {
            Signal<T>.Subscribe(name, callback, order);
        }

        public static void Subscribe<T1, T2>(Action<T1, T2> callback, int order = 0)
        {
            Signal<T1, T2>.Subscribe("", callback, order);
        }

        public static void Subscribe<T1, T2>(string name, Action<T1, T2> callback, int order = 0)
        {
            Signal<T1, T2>.Subscribe(name, callback, order);
        }

        public static void Subscribe<T1, T2, T3>(Action<T1, T2, T3> callback, int order = 0)
        {
            Signal<T1, T2, T3>.Subscribe("", callback, order);
        }

        public static void Subscribe<T1, T2, T3>(string name, Action<T1, T2, T3> callback, int order = 0)
        {
            Signal<T1, T2, T3>.Subscribe(name, callback, order);
        }

        public static void Subscribe<T1, T2, T3, T4>(Action<T1, T2, T3, T4> callback, int order = 0)
        {
            Signal<T1, T2, T3, T4>.Subscribe("", callback, order);
        }

        public static void Subscribe<T1, T2, T3, T4>(string name, Action<T1, T2, T3, T4> callback, int order = 0)
        {
            Signal<T1, T2, T3, T4>.Subscribe(name, callback, order);
        }

        public static void Subscribe<T1, T2, T3, T4, T5>(Action<T1, T2, T3, T4, T5> callback, int order = 0)
        {
            Signal<T1, T2, T3, T4, T5>.Subscribe("", callback, order);
        }

        public static void Subscribe<T1, T2, T3, T4, T5>(string name, Action<T1, T2, T3, T4, T5> callback, int order = 0)
        {
            Signal<T1, T2, T3, T4, T5>.Subscribe(name, callback, order);
        }

        public static void Subscribe<T1, T2, T3, T4, T5, T6>(Action<T1, T2, T3, T4, T5, T6> callback, int order = 0)
        {
            Signal<T1, T2, T3, T4, T5, T6>.Subscribe("", callback, order);
        }

        public static void Subscribe<T1, T2, T3, T4, T5, T6>(string name, Action<T1, T2, T3, T4, T5, T6> callback, int order = 0)
        {
            Signal<T1, T2, T3, T4, T5, T6>.Subscribe(name, callback, order);
        }

        public static void Subscribe<T1, T2, T3, T4, T5, T6, T7>(Action<T1, T2, T3, T4, T5, T6, T7> callback, int order = 0)
        {
            Signal<T1, T2, T3, T4, T5, T6, T7>.Subscribe("", callback, order);
        }

        public static void Subscribe<T1, T2, T3, T4, T5, T6, T7>(string name, Action<T1, T2, T3, T4, T5, T6, T7> callback, int order = 0)
        {
            Signal<T1, T2, T3, T4, T5, T6, T7>.Subscribe(name, callback, order);
        }

        public static void Subscribe<T1, T2, T3, T4, T5, T6, T7, T8>(Action<T1, T2, T3, T4, T5, T6, T7, T8> callback, int order = 0)
        {
            Signal<T1, T2, T3, T4, T5, T6, T7, T8>.Subscribe("", callback, order);
        }

        public static void Subscribe<T1, T2, T3, T4, T5, T6, T7, T8>(string name, Action<T1, T2, T3, T4, T5, T6, T7, T8> callback, int order = 0)
        {
            Signal<T1, T2, T3, T4, T5, T6, T7, T8>.Subscribe(name, callback, order);
        }

        public static void Subscribe<T1, T2, T3, T4, T5, T6, T7, T8, T9>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9> callback, int order = 0)
        {
            Signal<T1, T2, T3, T4, T5, T6, T7, T8, T9>.Subscribe("", callback, order);
        }

        public static void Subscribe<T1, T2, T3, T4, T5, T6, T7, T8, T9>(string name, Action<T1, T2, T3, T4, T5, T6, T7, T8, T9> callback, int order = 0)
        {
            Signal<T1, T2, T3, T4, T5, T6, T7, T8, T9>.Subscribe(name, callback, order);
        }

        public static void Subscribe<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> callback, int order = 0)
        {
            Signal<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>.Subscribe("", callback, order);
        }

        public static void Subscribe<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(string name, Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> callback, int order = 0)
        {
            Signal<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>.Subscribe(name, callback, order);
        }

        #endregion
    }
}