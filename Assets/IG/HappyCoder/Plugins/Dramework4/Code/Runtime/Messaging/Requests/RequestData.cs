using System;


namespace IG.HappyCoder.Plugins.Dramework4.Runtime.Messaging.Requests
{
    internal class RequestData<TOut>
    {
        #region ================================ FIELDS

        internal Func<TOut> Listener;

        #endregion
    }

    internal class RequestData<TIn, TOut>
    {
        #region ================================ FIELDS

        internal Func<TIn, TOut> Listener;

        #endregion
    }

    internal class RequestData<TIn1, TIn2, TOut>
    {
        #region ================================ FIELDS

        internal Func<TIn1, TIn2, TOut> Listener;

        #endregion
    }

    internal class RequestData<TIn1, TIn2, TIn3, TOut>
    {
        #region ================================ FIELDS

        internal Func<TIn1, TIn2, TIn3, TOut> Listener;

        #endregion
    }

    internal class RequestData<TIn1, TIn2, TIn3, TIn4, TOut>
    {
        #region ================================ FIELDS

        internal Func<TIn1, TIn2, TIn3, TIn4, TOut> Listener;

        #endregion
    }

    internal class RequestData<TIn1, TIn2, TIn3, TIn4, TIn5, TOut>
    {
        #region ================================ FIELDS

        internal Func<TIn1, TIn2, TIn3, TIn4, TIn5, TOut> Listener;

        #endregion
    }

    internal class RequestData<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, TOut>
    {
        #region ================================ FIELDS

        internal Func<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, TOut> Listener;

        #endregion
    }

    internal class RequestData<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, TIn7, TOut>
    {
        #region ================================ FIELDS

        internal Func<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, TIn7, TOut> Listener;

        #endregion
    }

    internal class RequestData<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, TIn7, TIn8, TOut>
    {
        #region ================================ FIELDS

        internal Func<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, TIn7, TIn8, TOut> Listener;

        #endregion
    }

    internal class RequestData<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, TIn7, TIn8, TIn9, TOut>
    {
        #region ================================ FIELDS

        internal Func<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, TIn7, TIn8, TIn9, TOut> Listener;

        #endregion
    }

    internal class RequestData<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, TIn7, TIn8, TIn9, T10, TOut>
    {
        #region ================================ FIELDS

        internal Func<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, TIn7, TIn8, TIn9, T10, TOut> Listener;

        #endregion
    }
}