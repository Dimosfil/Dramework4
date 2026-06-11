using System;

using Unity.IL2CPP.CompilerServices;


namespace IG.HappyCoder.Plugins.Dramework4.Runtime.Messaging.Requests
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public static partial class DWRequest
    {
        #region ================================ METHODS

        public static void Subscribe<TOut>(string name, Func<TOut> callback)
        {
            Request<TOut>.Subscribe(name, callback);
        }

        public static void Subscribe<TIn, TOut>(string name, Func<TIn, TOut> callback)
        {
            Request<TIn, TOut>.Subscribe(name, callback);
        }

        public static void Subscribe<TIn1, TIn2, TOut>(string name, Func<TIn1, TIn2, TOut> callback)
        {
            Request<TIn1, TIn2, TOut>.Subscribe(name, callback);
        }

        public static void Subscribe<TIn1, TIn2, TIn3, TOut>(string name, Func<TIn1, TIn2, TIn3, TOut> callback)
        {
            Request<TIn1, TIn2, TIn3, TOut>.Subscribe(name, callback);
        }

        public static void Subscribe<TIn1, TIn2, TIn3, TIn4, TOut>(string name, Func<TIn1, TIn2, TIn3, TIn4, TOut> callback)
        {
            Request<TIn1, TIn2, TIn3, TIn4, TOut>.Subscribe(name, callback);
        }

        public static void Subscribe<TIn1, TIn2, TIn3, TIn4, TIn5, TOut>(string name, Func<TIn1, TIn2, TIn3, TIn4, TIn5, TOut> callback)
        {
            Request<TIn1, TIn2, TIn3, TIn4, TIn5, TOut>.Subscribe(name, callback);
        }

        public static void Subscribe<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, TOut>(string name, Func<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, TOut> callback)
        {
            Request<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, TOut>.Subscribe(name, callback);
        }

        public static void Subscribe<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, TIn7, TOut>(string name, Func<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, TIn7, TOut> callback)
        {
            Request<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, TIn7, TOut>.Subscribe(name, callback);
        }

        public static void Subscribe<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, TIn7, TIn8, TOut>(string name, Func<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, TIn7, TIn8, TOut> callback)
        {
            Request<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, TIn7, TIn8, TOut>.Subscribe(name, callback);
        }

        public static void Subscribe<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, TIn7, TIn8, TIn9, TOut>(string name, Func<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, TIn7, TIn8, TIn9, TOut> callback)
        {
            Request<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, TIn7, TIn8, TIn9, TOut>.Subscribe(name, callback);
        }

        public static void Subscribe<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, TIn7, TIn8, TIn9, TIn10, TOut>(string name, Func<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, TIn7, TIn8, TIn9, TIn10, TOut> callback)
        {
            Request<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, TIn7, TIn8, TIn9, TIn10, TOut>.Subscribe(name, callback);
        }

        #endregion
    }
}