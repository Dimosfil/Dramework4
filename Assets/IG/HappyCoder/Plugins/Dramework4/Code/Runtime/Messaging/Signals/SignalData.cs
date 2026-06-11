using System;
using System.Collections.Generic;
using System.Linq;


namespace IG.HappyCoder.Plugins.Dramework4.Runtime.Messaging.Signals
{
    internal class SignalData
    {
        #region ================================ FIELDS

        internal readonly string Name;

        private List<(int order, Action callback)> _listeners = new List<(int order, Action callback)>();

        #endregion

        #region ================================ CONSTRUCTORS AND DESTRUCTOR

        internal SignalData(string name)
        {
            Name = name;
        }

        #endregion

        #region ================================ PROPERTIES AND INDEXERS

        internal bool HasListeners => _listeners.Count > 0;
        internal bool IsLock { get; private set; }

        #endregion

        #region ================================ METHODS

        internal void Invoke()
        {
            foreach (var listener in _listeners)
                listener.callback.Invoke();
        }

        internal void Lock()
        {
            IsLock = true;
        }

        internal void Subscribe(Action callback, int order)
        {
            _listeners.Add((order, callback));
            if (order != 0)
                _listeners = _listeners.OrderBy(i => i.order).ToList();
        }

        internal void Unlock()
        {
            IsLock = false;
        }

        internal void Unsubscribe(Action callback)
        {
            for (var i = _listeners.Count - 1; i >= 0; i--)
            {
                if (_listeners[i].callback != callback) continue;
                _listeners.RemoveAt(i);
            }
        }

        #endregion
    }

    internal class SignalData<T>
    {
        #region ================================ FIELDS

        internal readonly string Name;
        private List<(int order, Action<T> callback)> _listeners = new List<(int order, Action<T> callback)>();

        #endregion

        #region ================================ CONSTRUCTORS AND DESTRUCTOR

        internal SignalData(string name)
        {
            Name = name;
        }

        #endregion

        #region ================================ PROPERTIES AND INDEXERS

        internal bool HasListeners => _listeners.Count > 0;
        internal bool IsLock { get; private set; }

        #endregion

        #region ================================ METHODS

        internal void Invoke(T data)
        {
            foreach (var listener in _listeners)
                listener.callback.Invoke(data);
        }

        internal void Lock()
        {
            IsLock = true;
        }

        internal void Subscribe(Action<T> callback, int order)
        {
            _listeners.Add((order, callback));
            if (order != 0)
                _listeners = _listeners.OrderBy(i => i.order).ToList();
        }

        internal void Unlock()
        {
            IsLock = false;
        }

        internal void Unsubscribe(Action<T> callback)
        {
            for (var i = _listeners.Count - 1; i >= 0; i--)
            {
                if (_listeners[i].callback != callback) continue;
                _listeners.RemoveAt(i);
            }
        }

        #endregion
    }

    internal class SignalData<T1, T2>
    {
        #region ================================ FIELDS

        private List<(int order, Action<T1, T2> callback)> _listeners = new List<(int order, Action<T1, T2> callback)>();

        #endregion

        #region ================================ CONSTRUCTORS AND DESTRUCTOR

        internal SignalData(string name)
        {
            Name = name;
        }

        #endregion

        #region ================================ PROPERTIES AND INDEXERS

        internal bool HasListeners => _listeners.Count > 0;
        internal bool IsLock { get; private set; }

        internal string Name { get; private set; }

        #endregion

        #region ================================ METHODS

        internal void Invoke(T1 data1, T2 data2)
        {
            foreach (var listener in _listeners)
                listener.callback.Invoke(data1, data2);
        }

        internal void Lock()
        {
            IsLock = true;
        }

        internal void Subscribe(Action<T1, T2> callback, int order)
        {
            _listeners.Add((order, callback));
            if (order != 0)
                _listeners = _listeners.OrderBy(i => i.order).ToList();
        }

        internal void Unlock()
        {
            IsLock = false;
        }

        internal void Unsubscribe(Action<T1, T2> callback)
        {
            for (var i = _listeners.Count - 1; i >= 0; i--)
            {
                if (_listeners[i].callback != callback) continue;
                _listeners.RemoveAt(i);
            }
        }

        #endregion
    }

    internal class SignalData<T1, T2, T3>
    {
        #region ================================ FIELDS

        private List<(int order, Action<T1, T2, T3> callback)> _listeners = new List<(int order, Action<T1, T2, T3> callback)>();

        #endregion

        #region ================================ CONSTRUCTORS AND DESTRUCTOR

        internal SignalData(string name)
        {
            Name = name;
        }

        #endregion

        #region ================================ PROPERTIES AND INDEXERS

        internal bool HasListeners => _listeners.Count > 0;
        internal bool IsLock { get; private set; }
        internal string Name { get; private set; }

        #endregion

        #region ================================ METHODS

        internal void Invoke(T1 data1, T2 data2, T3 data3)
        {
            foreach (var listener in _listeners)
                listener.callback.Invoke(data1, data2, data3);
        }

        internal void Lock()
        {
            IsLock = true;
        }

        internal void Subscribe(Action<T1, T2, T3> callback, int order)
        {
            _listeners.Add((order, callback));
            if (order != 0)
                _listeners = _listeners.OrderBy(i => i.order).ToList();
        }

        internal void Unlock()
        {
            IsLock = false;
        }

        internal void Unsubscribe(Action<T1, T2, T3> callback)
        {
            for (var i = _listeners.Count - 1; i >= 0; i--)
            {
                if (_listeners[i].callback != callback) continue;
                _listeners.RemoveAt(i);
            }
        }

        #endregion
    }

    internal class SignalData<T1, T2, T3, T4>
    {
        #region ================================ FIELDS

        internal readonly string Name;
        internal bool Lock;

        private List<(int order, Action<T1, T2, T3, T4> callback)> _listeners = new List<(int order, Action<T1, T2, T3, T4> callback)>();

        #endregion

        #region ================================ CONSTRUCTORS AND DESTRUCTOR

        internal SignalData(string name)
        {
            Name = name;
        }

        #endregion

        #region ================================ PROPERTIES AND INDEXERS

        internal bool HasListeners => _listeners.Count > 0;

        #endregion

        #region ================================ METHODS

        internal void Invoke(T1 data1, T2 data2, T3 data3, T4 data4)
        {
            foreach (var listener in _listeners)
                listener.callback.Invoke(data1, data2, data3, data4);
        }

        internal void Subscribe(Action<T1, T2, T3, T4> callback, int order)
        {
            _listeners.Add((order, callback));
            if (order != 0)
                _listeners = _listeners.OrderBy(i => i.order).ToList();
        }

        internal void Unsubscribe(Action<T1, T2, T3, T4> callback)
        {
            for (var i = _listeners.Count - 1; i >= 0; i--)
            {
                if (_listeners[i].callback != callback) continue;
                _listeners.RemoveAt(i);
            }
        }

        #endregion
    }

    internal class SignalData<T1, T2, T3, T4, T5>
    {
        #region ================================ FIELDS

        internal readonly string Name;
        internal bool Lock;

        private List<(int order, Action<T1, T2, T3, T4, T5> callback)> _listeners = new List<(int order, Action<T1, T2, T3, T4, T5> callback)>();

        #endregion

        #region ================================ CONSTRUCTORS AND DESTRUCTOR

        internal SignalData(string name)
        {
            Name = name;
        }

        #endregion

        #region ================================ PROPERTIES AND INDEXERS

        internal bool HasListeners => _listeners.Count > 0;

        #endregion

        #region ================================ METHODS

        internal void Invoke(T1 data1, T2 data2, T3 data3, T4 data4, T5 data5)
        {
            foreach (var listener in _listeners)
                listener.callback.Invoke(data1, data2, data3, data4, data5);
        }

        internal void Subscribe(Action<T1, T2, T3, T4, T5> callback, int order)
        {
            _listeners.Add((order, callback));
            if (order != 0)
                _listeners = _listeners.OrderBy(i => i.order).ToList();
        }

        internal void Unsubscribe(Action<T1, T2, T3, T4, T5> callback)
        {
            for (var i = _listeners.Count - 1; i >= 0; i--)
            {
                if (_listeners[i].callback != callback) continue;
                _listeners.RemoveAt(i);
            }
        }

        #endregion
    }

    internal class SignalData<T1, T2, T3, T4, T5, T6>
    {
        #region ================================ FIELDS

        internal readonly string Name;
        internal bool Lock;

        private List<(int order, Action<T1, T2, T3, T4, T5, T6> callback)> _listeners = new List<(int order, Action<T1, T2, T3, T4, T5, T6> callback)>();

        #endregion

        #region ================================ CONSTRUCTORS AND DESTRUCTOR

        internal SignalData(string name)
        {
            Name = name;
        }

        #endregion

        #region ================================ PROPERTIES AND INDEXERS

        internal bool HasListeners => _listeners.Count > 0;

        #endregion

        #region ================================ METHODS

        internal void Invoke(T1 data1, T2 data2, T3 data3, T4 data4, T5 data5, T6 data6)
        {
            foreach (var listener in _listeners)
                listener.callback.Invoke(data1, data2, data3, data4, data5, data6);
        }

        internal void Subscribe(Action<T1, T2, T3, T4, T5, T6> callback, int order)
        {
            _listeners.Add((order, callback));
            if (order != 0)
                _listeners = _listeners.OrderBy(i => i.order).ToList();
        }

        internal void Unsubscribe(Action<T1, T2, T3, T4, T5, T6> callback)
        {
            for (var i = _listeners.Count - 1; i >= 0; i--)
            {
                if (_listeners[i].callback != callback) continue;
                _listeners.RemoveAt(i);
            }
        }

        #endregion
    }

    internal class SignalData<T1, T2, T3, T4, T5, T6, T7>
    {
        #region ================================ FIELDS

        internal readonly string Name;
        internal bool Lock;

        private List<(int order, Action<T1, T2, T3, T4, T5, T6, T7> callback)> _listeners = new List<(int order, Action<T1, T2, T3, T4, T5, T6, T7> callback)>();

        #endregion

        #region ================================ CONSTRUCTORS AND DESTRUCTOR

        internal SignalData(string name)
        {
            Name = name;
        }

        #endregion

        #region ================================ PROPERTIES AND INDEXERS

        internal bool HasListeners => _listeners.Count > 0;

        #endregion

        #region ================================ METHODS

        internal void Invoke(T1 data1, T2 data2, T3 data3, T4 data4, T5 data5, T6 data6, T7 data7)
        {
            foreach (var listener in _listeners)
                listener.callback.Invoke(data1, data2, data3, data4, data5, data6, data7);
        }

        internal void Subscribe(Action<T1, T2, T3, T4, T5, T6, T7> callback, int order)
        {
            _listeners.Add((order, callback));
            if (order != 0)
                _listeners = _listeners.OrderBy(i => i.order).ToList();
        }

        internal void Unsubscribe(Action<T1, T2, T3, T4, T5, T6, T7> callback)
        {
            for (var i = _listeners.Count - 1; i >= 0; i--)
            {
                if (_listeners[i].callback != callback) continue;
                _listeners.RemoveAt(i);
            }
        }

        #endregion
    }

    internal class SignalData<T1, T2, T3, T4, T5, T6, T7, T8>
    {
        #region ================================ FIELDS

        internal readonly string Name;
        internal bool Lock;

        private List<(int order, Action<T1, T2, T3, T4, T5, T6, T7, T8> callback)> _listeners = new List<(int order, Action<T1, T2, T3, T4, T5, T6, T7, T8> callback)>();

        #endregion

        #region ================================ CONSTRUCTORS AND DESTRUCTOR

        internal SignalData(string name)
        {
            Name = name;
        }

        #endregion

        #region ================================ PROPERTIES AND INDEXERS

        internal bool HasListeners => _listeners.Count > 0;

        #endregion

        #region ================================ METHODS

        internal void Invoke(T1 data1, T2 data2, T3 data3, T4 data4, T5 data5, T6 data6, T7 data7, T8 data8)
        {
            foreach (var listener in _listeners)
                listener.callback.Invoke(data1, data2, data3, data4, data5, data6, data7, data8);
        }

        internal void Subscribe(Action<T1, T2, T3, T4, T5, T6, T7, T8> callback, int order)
        {
            _listeners.Add((order, callback));
            if (order != 0)
                _listeners = _listeners.OrderBy(i => i.order).ToList();
        }

        internal void Unsubscribe(Action<T1, T2, T3, T4, T5, T6, T7, T8> callback)
        {
            for (var i = _listeners.Count - 1; i >= 0; i--)
            {
                if (_listeners[i].callback != callback) continue;
                _listeners.RemoveAt(i);
            }
        }

        #endregion
    }

    internal class SignalData<T1, T2, T3, T4, T5, T6, T7, T8, T9>
    {
        #region ================================ FIELDS

        internal readonly string Name;
        internal bool Lock;

        private List<(int order, Action<T1, T2, T3, T4, T5, T6, T7, T8, T9> callback)> _listeners = new List<(int order, Action<T1, T2, T3, T4, T5, T6, T7, T8, T9> callback)>();

        #endregion

        #region ================================ CONSTRUCTORS AND DESTRUCTOR

        internal SignalData(string name)
        {
            Name = name;
        }

        #endregion

        #region ================================ PROPERTIES AND INDEXERS

        internal bool HasListeners => _listeners.Count > 0;

        #endregion

        #region ================================ METHODS

        internal void Invoke(T1 data1, T2 data2, T3 data3, T4 data4, T5 data5, T6 data6, T7 data7, T8 data8, T9 data9)
        {
            foreach (var listener in _listeners)
                listener.callback.Invoke(data1, data2, data3, data4, data5, data6, data7, data8, data9);
        }

        internal void Subscribe(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9> callback, int order)
        {
            _listeners.Add((order, callback));
            if (order != 0)
                _listeners = _listeners.OrderBy(i => i.order).ToList();
        }

        internal void Unsubscribe(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9> callback)
        {
            for (var i = _listeners.Count - 1; i >= 0; i--)
            {
                if (_listeners[i].callback != callback) continue;
                _listeners.RemoveAt(i);
            }
        }

        #endregion
    }

    internal class SignalData<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>
    {
        #region ================================ FIELDS

        internal readonly string Name;
        internal bool Lock;

        private List<(int order, Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> callback)> _listeners = new List<(int order, Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> callback)>();

        #endregion

        #region ================================ CONSTRUCTORS AND DESTRUCTOR

        internal SignalData(string name)
        {
            Name = name;
        }

        #endregion

        #region ================================ PROPERTIES AND INDEXERS

        internal bool HasListeners => _listeners.Count > 0;

        #endregion

        #region ================================ METHODS

        internal void Invoke(T1 data1, T2 data2, T3 data3, T4 data4, T5 data5, T6 data6, T7 data7, T8 data8, T9 data9, T10 data10)
        {
            foreach (var listener in _listeners)
                listener.callback.Invoke(data1, data2, data3, data4, data5, data6, data7, data8, data9, data10);
        }

        internal void Subscribe(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> callback, int order)
        {
            _listeners.Add((order, callback));
            if (order != 0)
                _listeners = _listeners.OrderBy(i => i.order).ToList();
        }

        internal void Unsubscribe(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> callback)
        {
            for (var i = _listeners.Count - 1; i >= 0; i--)
            {
                if (_listeners[i].callback != callback) continue;
                _listeners.RemoveAt(i);
            }
        }

        #endregion
    }
}