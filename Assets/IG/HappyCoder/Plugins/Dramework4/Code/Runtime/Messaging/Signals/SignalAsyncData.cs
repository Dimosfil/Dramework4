namespace IG.HappyCoder.Plugins.Dramework4.Runtime.Messaging.Signals
{
    internal class SignalAsyncData<T>
    {
        #region ================================ FIELDS

        private readonly SignalData<T> _signal;
        private readonly T _data;

        #endregion

        #region ================================ CONSTRUCTORS AND DESTRUCTOR

        internal SignalAsyncData(SignalData<T> signal, T data)
        {
            _signal = signal;
            _data = data;
        }

        #endregion

        #region ================================ PROPERTIES AND INDEXERS

        internal bool HasListeners => _signal.HasListeners;
        internal string Name => _signal.Name;

        #endregion

        #region ================================ METHODS

        internal void Invoke()
        {
            _signal.Invoke(_data);
        }

        internal void Lock()
        {
            _signal.Lock();
        }

        internal void Unlock()
        {
            _signal.Unlock();
        }

        #endregion
    }

    internal class SignalAsyncData<T1, T2>
    {
        #region ================================ FIELDS

        private readonly SignalData<T1, T2> _signal;
        private readonly T1 _data1;
        private readonly T2 _data2;

        #endregion

        #region ================================ CONSTRUCTORS AND DESTRUCTOR

        internal SignalAsyncData(SignalData<T1, T2> signal, T1 data1, T2 data2)
        {
            _signal = signal;
            _data1 = data1;
            _data2 = data2;
        }

        #endregion

        #region ================================ PROPERTIES AND INDEXERS

        internal bool HasListeners => _signal.HasListeners;
        internal string Name => _signal.Name;

        #endregion

        #region ================================ METHODS

        internal void Invoke()
        {
            _signal.Invoke(_data1, _data2);
        }

        internal void Lock()
        {
            _signal.Lock();
        }

        internal void Unlock()
        {
            _signal.Unlock();
        }

        #endregion
    }

    internal class SignalAsyncData<T1, T2, T3>
    {
        #region ================================ FIELDS

        private readonly SignalData<T1, T2, T3> _signal;
        private readonly T1 _data1;
        private readonly T2 _data2;
        private readonly T3 _data3;

        #endregion

        #region ================================ CONSTRUCTORS AND DESTRUCTOR

        internal SignalAsyncData(SignalData<T1, T2, T3> signal, T1 data1, T2 data2, T3 data3)
        {
            _signal = signal;
            _data1 = data1;
            _data2 = data2;
            _data3 = data3;
        }

        #endregion

        #region ================================ PROPERTIES AND INDEXERS

        internal bool HasListeners => _signal.HasListeners;
        internal string Name => _signal.Name;

        #endregion

        #region ================================ METHODS

        internal void Invoke()
        {
            _signal.Invoke(_data1, _data2, _data3);
        }

        internal void Lock()
        {
            _signal.Lock();
        }

        internal void Unlock()
        {
            _signal.Unlock();
        }

        #endregion
    }
}
