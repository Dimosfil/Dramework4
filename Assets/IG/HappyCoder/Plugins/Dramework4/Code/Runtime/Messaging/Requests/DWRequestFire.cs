using Unity.IL2CPP.CompilerServices;


namespace IG.HappyCoder.Plugins.Dramework4.Runtime.Messaging.Requests
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public static partial class DWRequest
    {
        #region ================================ METHODS

        public static TOut Fire<TOut>(string name)
        {
            return Request<TOut>.Fire(name);
        }

        public static TOut Fire<TIn, TOut>(string name, TIn data)
        {
            return Request<TIn, TOut>.Fire(name, data);
        }

        public static TOut Fire<TIn1, TIn2, TOut>(string name, TIn1 data1, TIn2 data2)
        {
            return Request<TIn1, TIn2, TOut>.Fire(name, data1, data2);
        }

        public static TOut Fire<TIn1, TIn2, TIn3, TOut>(string name, TIn1 data1, TIn2 data2, TIn3 data3)
        {
            return Request<TIn1, TIn2, TIn3, TOut>.Fire(name, data1, data2, data3);
        }

        public static TOut Fire<TIn1, TIn2, TIn3, TIn4, TOut>(string name, TIn1 data1, TIn2 data2, TIn3 data3, TIn4 data4)
        {
            return Request<TIn1, TIn2, TIn3, TIn4, TOut>.Fire(name, data1, data2, data3, data4);
        }

        public static TOut Fire<TIn1, TIn2, TIn3, TIn4, TIn5, TOut>(string name, TIn1 data1, TIn2 data2, TIn3 data3, TIn4 data4, TIn5 data5)
        {
            return Request<TIn1, TIn2, TIn3, TIn4, TIn5, TOut>.Fire(name, data1, data2, data3, data4, data5);
        }

        public static TOut Fire<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, TOut>(string name, TIn1 data1, TIn2 data2, TIn3 data3, TIn4 data4, TIn5 data5, TIn6 data6)
        {
            return Request<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, TOut>.Fire(name, data1, data2, data3, data4, data5, data6);
        }

        public static TOut Fire<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, TIn7, TOut>(string name, TIn1 data1, TIn2 data2, TIn3 data3, TIn4 data4, TIn5 data5, TIn6 data6, TIn7 data7)
        {
            return Request<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, TIn7, TOut>.Fire(name, data1, data2, data3, data4, data5, data6, data7);
        }

        public static TOut Fire<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, TIn7, TIn8, TOut>(string name, TIn1 data1, TIn2 data2, TIn3 data3, TIn4 data4, TIn5 data5, TIn6 data6, TIn7 data7, TIn8 data8)
        {
            return Request<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, TIn7, TIn8, TOut>.Fire(name, data1, data2, data3, data4, data5, data6, data7, data8);
        }

        public static TOut Fire<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, TIn7, TIn8, TIn9, TOut>(string name, TIn1 data1, TIn2 data2, TIn3 data3, TIn4 data4, TIn5 data5, TIn6 data6, TIn7 data7, TIn8 data8, TIn9 data9)
        {
            return Request<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, TIn7, TIn8, TIn9, TOut>.Fire(name, data1, data2, data3, data4, data5, data6, data7, data8, data9);
        }

        public static TOut Fire<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, TIn7, TIn8, TIn9, TIn10, TOut>(string name, TIn1 data1, TIn2 data2, TIn3 data3, TIn4 data4, TIn5 data5, TIn6 data6, TIn7 data7, TIn8 data8, TIn9 data9, TIn10 data10)
        {
            return Request<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, TIn7, TIn8, TIn9, TIn10, TOut>.Fire(name, data1, data2, data3, data4, data5, data6, data7, data8, data9, data10);
        }

        #endregion
    }
}