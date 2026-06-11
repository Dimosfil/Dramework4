using Unity.IL2CPP.CompilerServices;


namespace IG.HappyCoder.Plugins.Dramework4.Runtime.Messaging.Signals
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public static partial class DWSignal
    {
        #region ================================ METHODS

        public static void Fire(string name)
        {
            Signal.Fire(name);
        }

        public static void Fire<T>()
        {
            Signal.Fire<T>();
        }

        public static void Fire<T>(T data)
        {
            Signal<T>.Fire("", data);
        }

        public static void Fire<T>(string name, T data)
        {
            Signal<T>.Fire(name, data);
        }

        public static void Fire<T1, T2>(T1 data1, T2 data2)
        {
            Signal<T1, T2>.Fire("", data1, data2);
        }

        public static void Fire<T1, T2>(string name, T1 data1, T2 data2)
        {
            Signal<T1, T2>.Fire(name, data1, data2);
        }

        public static void Fire<T1, T2, T3>(T1 data1, T2 data2, T3 data3)
        {
            Signal<T1, T2, T3>.Fire("", data1, data2, data3);
        }

        public static void Fire<T1, T2, T3>(string name, T1 data1, T2 data2, T3 data3)
        {
            Signal<T1, T2, T3>.Fire(name, data1, data2, data3);
        }

        public static void Fire<T1, T2, T3, T4>(T1 data1, T2 data2, T3 data3, T4 data4)
        {
            Signal<T1, T2, T3, T4>.Fire("", data1, data2, data3, data4);
        }

        public static void Fire<T1, T2, T3, T4>(string name, T1 data1, T2 data2, T3 data3, T4 data4)
        {
            Signal<T1, T2, T3, T4>.Fire(name, data1, data2, data3, data4);
        }

        public static void Fire<T1, T2, T3, T4, T5>(T1 data1, T2 data2, T3 data3, T4 data4, T5 data5)
        {
            Signal<T1, T2, T3, T4, T5>.Fire("", data1, data2, data3, data4, data5);
        }

        public static void Fire<T1, T2, T3, T4, T5>(string name, T1 data1, T2 data2, T3 data3, T4 data4, T5 data5)
        {
            Signal<T1, T2, T3, T4, T5>.Fire(name, data1, data2, data3, data4, data5);
        }

        public static void Fire<T1, T2, T3, T4, T5, T6>(T1 data1, T2 data2, T3 data3, T4 data4, T5 data5, T6 data6)
        {
            Signal<T1, T2, T3, T4, T5, T6>.Fire("", data1, data2, data3, data4, data5, data6);
        }

        public static void Fire<T1, T2, T3, T4, T5, T6>(string name, T1 data1, T2 data2, T3 data3, T4 data4, T5 data5, T6 data6)
        {
            Signal<T1, T2, T3, T4, T5, T6>.Fire(name, data1, data2, data3, data4, data5, data6);
        }

        public static void Fire<T1, T2, T3, T4, T5, T6, T7>(T1 data1, T2 data2, T3 data3, T4 data4, T5 data5, T6 data6, T7 data7)
        {
            Signal<T1, T2, T3, T4, T5, T6, T7>.Fire("", data1, data2, data3, data4, data5, data6, data7);
        }

        public static void Fire<T1, T2, T3, T4, T5, T6, T7>(string name, T1 data1, T2 data2, T3 data3, T4 data4, T5 data5, T6 data6, T7 data7)
        {
            Signal<T1, T2, T3, T4, T5, T6, T7>.Fire(name, data1, data2, data3, data4, data5, data6, data7);
        }

        public static void Fire<T1, T2, T3, T4, T5, T6, T7, T8>(T1 data1, T2 data2, T3 data3, T4 data4, T5 data5, T6 data6, T7 data7, T8 data8)
        {
            Signal<T1, T2, T3, T4, T5, T6, T7, T8>.Fire("", data1, data2, data3, data4, data5, data6, data7, data8);
        }

        public static void Fire<T1, T2, T3, T4, T5, T6, T7, T8>(string name, T1 data1, T2 data2, T3 data3, T4 data4, T5 data5, T6 data6, T7 data7, T8 data8)
        {
            Signal<T1, T2, T3, T4, T5, T6, T7, T8>.Fire(name, data1, data2, data3, data4, data5, data6, data7, data8);
        }

        public static void Fire<T1, T2, T3, T4, T5, T6, T7, T8, T9>(T1 data1, T2 data2, T3 data3, T4 data4, T5 data5, T6 data6, T7 data7, T8 data8, T9 data9)
        {
            Signal<T1, T2, T3, T4, T5, T6, T7, T8, T9>.Fire("", data1, data2, data3, data4, data5, data6, data7, data8, data9);
        }

        public static void Fire<T1, T2, T3, T4, T5, T6, T7, T8, T9>(string name, T1 data1, T2 data2, T3 data3, T4 data4, T5 data5, T6 data6, T7 data7, T8 data8, T9 data9)
        {
            Signal<T1, T2, T3, T4, T5, T6, T7, T8, T9>.Fire(name, data1, data2, data3, data4, data5, data6, data7, data8, data9);
        }

        public static void Fire<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(T1 data1, T2 data2, T3 data3, T4 data4, T5 data5, T6 data6, T7 data7, T8 data8, T9 data9, T10 data10)
        {
            Signal<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>.Fire("", data1, data2, data3, data4, data5, data6, data7, data8, data9, data10);
        }

        public static void Fire<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(string name, T1 data1, T2 data2, T3 data3, T4 data4, T5 data5, T6 data6, T7 data7, T8 data8, T9 data9, T10 data10)
        {
            Signal<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>.Fire(name, data1, data2, data3, data4, data5, data6, data7, data8, data9, data10);
        }

        #endregion
    }
}