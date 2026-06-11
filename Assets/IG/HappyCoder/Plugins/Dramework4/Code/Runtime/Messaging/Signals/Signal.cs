using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

using IG.HappyCoder.Plugins.Dramework4.Runtime.Core;
using IG.HappyCoder.Plugins.Dramework4.Runtime.Tools;

using Unity.IL2CPP.CompilerServices;


namespace IG.HappyCoder.Plugins.Dramework4.Runtime.Messaging.Signals
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    [SuppressMessage("ReSharper", "StaticMemberInGenericType")]
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
    internal static class Signal
    {
        #region ================================ FIELDS

        private static readonly Dictionary<string, SignalData> _signals = new Dictionary<string, SignalData>();
        private static Guid _sessionID;

        #endregion

        #region ================================ CONSTRUCTORS AND DESTRUCTOR

        static Signal()
        {
            CheckSession();
        }

        #endregion

        #region ================================ METHODS

        internal static void Fire(string name)
        {
            GetSignal(name, out var signal);

            if (signal.HasListeners == false)
            {
                if (DW4.AppConfig.LogDispatcher)
                    DW4.LogError($"[Signal.Fire]: Signal, Name:«{name}» has no listeners. Unable to fire signal");
                return;
            }

            if (DW4.AppConfig.LogDispatcher)
                DW4.Log($"[Signal.Fire]: Signal, Name:«{name}» is fired");

            signal.Lock();
            signal.Invoke();
            signal.Unlock();
        }

        internal static void Fire<T>()
        {
            var name = GetName<T>();

            GetSignal(name, out var signal);

            if (signal.HasListeners == false)
            {
                if (DW4.AppConfig.LogDispatcher)
                    DW4.LogError($"[Signal.Fire]: Signal, Name:«{name}» has no listeners. Unable to fire signal");
                return;
            }

            if (DW4.AppConfig.LogDispatcher)
                DW4.Log($"[Signal.Fire]: Signal, Name:«{name}» is fired");

            signal.Lock();
            signal.Invoke();
            signal.Unlock();
        }

        internal static void Subscribe(string name, Action callback, int order)
        {
            GetSignal(name, out var signal);

            if (DW4.AppConfig.LogDispatcher)
                DW4.Log($"[Signal.Subscribe]: Listener have been added to the signals, Name:«{name}»");

            signal.Subscribe(callback, order);
        }

        internal static void Subscribe<T>(Action callback, int order)
        {
            var name = GetName<T>();
            Subscribe(name, callback, order);
        }

        internal static void Unsubscribe(string name, Action callback)
        {
            if (_signals.TryGetValue(name, out var signalData))
            {
                if (DW4.AppConfig.LogDispatcher)
                    DW4.Log($"[Signal.Unsubscribe]: Listener have been removed from the signals, Name:«{name}»");

                signalData.Unsubscribe(callback);
            }
            else
            {
                if (DW4.AppConfig.LogDispatcher)
                    DW4.LogError($"[Signal.Unsubscribe]: Any signal, Name:«{name}» is not found");
            }
        }

        internal static void Unsubscribe<T>(Action callback)
        {
            var name = GetName<T>();

            if (_signals.TryGetValue(name, out var signalData))
            {
                if (DW4.AppConfig.LogDispatcher)
                    DW4.Log($"[Signal.Unsubscribe]: Listener have been removed from the signals, Name:«{name}»");

                signalData.Unsubscribe(callback);
            }
            else
            {
                if (DW4.AppConfig.LogDispatcher)
                    DW4.LogError($"[Signal.Unsubscribe]: Any signal, Name:«{name}» is not found");
            }
        }

        private static void CheckSession()
        {
#if UNITY_EDITOR
            if (Dispatcher.Instance == null) return;

            if (Dispatcher.Instance.SessionID == _sessionID) return;
            _signals.Clear();
            _sessionID = Dispatcher.Instance.SessionID;
#endif
        }

        private static string GetName<T>()
        {
            return typeof(T).AssemblyQualifiedName;
        }

        private static void GetSignal(string name, out SignalData signal)
        {
            if (_signals.TryGetValue(name, out signal)) return;
            signal = new SignalData(name);
            _signals.Add(name, signal);
        }

        #endregion
    }

    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    [SuppressMessage("ReSharper", "StaticMemberInGenericType")]
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
    internal class Signal<T>
    {
        #region ================================ FIELDS

        private static readonly Dictionary<string, SignalData<T>> _signals = new Dictionary<string, SignalData<T>>();
        private static Guid _sessionID;

        #endregion

        #region ================================ CONSTRUCTORS AND DESTRUCTOR

        static Signal()
        {
            CheckSession();
        }

        #endregion

        #region ================================ METHODS

        internal static void Fire(string name, T data)
        {
            GetSignal(name, out var signal);

            if (signal.HasListeners == false)
            {
                if (DW4.AppConfig.LogDispatcher)
                    DW4.LogError($"[Signal.Fire]: Signal, Name:«{name}» has no listeners. Unable to fire signal");
                return;
            }

            if (DW4.AppConfig.LogDispatcher)
                DW4.Log($"[Signal.Fire]: Signal, Name:«{name}» is fired");

            signal.Lock();
            signal.Invoke(data);
            signal.Unlock();
        }

        internal static void Subscribe(string name, Action<T> callback, int order)
        {
            if (_signals.TryGetValue(name, out var signalData) == false)
            {
                signalData = new SignalData<T>(name);
                _signals.Add(name, signalData);
            }

            if (DW4.AppConfig.LogDispatcher)
                DW4.Log($"[Signal.Subscribe]: Listener have been added to the signals, Name:«{name}»");

            signalData.Subscribe(callback, order);
        }

        internal static void Unsubscribe(string name, Action<T> callback)
        {
            if (_signals.TryGetValue(name, out var signalData))
            {
                if (DW4.AppConfig.LogDispatcher)
                    DW4.Log($"[Signal.Unsubscribe]: Listener have been removed from the signals, Name:«{name}»");

                signalData.Unsubscribe(callback);
            }
            else
            {
                if (DW4.AppConfig.LogDispatcher)
                    DW4.LogError($"[Signal.Unsubscribe]: Any signal, Name:«{name}» is not found");
            }
        }

        private static void CheckSession()
        {
#if UNITY_EDITOR
            if (Dispatcher.Instance == null) return;

            if (Dispatcher.Instance.SessionID == _sessionID) return;
            _signals.Clear();
            _sessionID = Dispatcher.Instance.SessionID;
#endif
        }

        private static void GetSignal(string name, out SignalData<T> signalData)
        {
            if (_signals.TryGetValue(name, out signalData)) return;
            signalData = new SignalData<T>(name);
            _signals.Add(name, signalData);
        }

        #endregion
    }

    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    [SuppressMessage("ReSharper", "StaticMemberInGenericType")]
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
    internal class Signal<T1, T2>
    {
        #region ================================ FIELDS

        private static readonly Dictionary<string, SignalData<T1, T2>> _signals = new Dictionary<string, SignalData<T1, T2>>();
        private static Guid _sessionID;

        #endregion

        #region ================================ METHODS

        internal static void Fire(string name, T1 data1, T2 data2)
        {
            CheckSession();
            GetData(name, out var signal);

            if (signal.HasListeners == false)
            {
                if (DW4.AppConfig.LogDispatcher)
                    DW4.LogError($"[Signal.Fire]: Signal, Name:«{name}» has no listeners. Unable to fire signal");
                return;
            }

            if (DW4.AppConfig.LogDispatcher)
                DW4.Log($"[Signal.Fire]: Signal, Name:«{name}» is fired");

            signal.Lock();
            signal.Invoke(data1, data2);
            signal.Unlock();
        }

        internal static void Subscribe(string name, Action<T1, T2> callback, int order)
        {
            CheckSession();

            if (_signals.TryGetValue(name, out var signalData) == false)
            {
                signalData = new SignalData<T1, T2>(name);
                _signals.Add(name, signalData);
            }

            if (DW4.AppConfig.LogDispatcher)
                DW4.Log($"[Signal.Subscribe]: Listener have been added to the signals, Name:«{name}»");

            signalData.Subscribe(callback, order);
        }

        internal static void Unsubscribe(string name, Action<T1, T2> callback)
        {
            if (_signals.TryGetValue(name, out var signalData))
            {
                if (DW4.AppConfig.LogDispatcher)
                    DW4.Log($"[Signal.Unsubscribe]: Listener have been removed from the signals, Name:«{name}»");

                signalData.Unsubscribe(callback);
            }
            else
            {
                if (DW4.AppConfig.LogDispatcher)
                    DW4.LogError($"[Signal.Unsubscribe]: Any signal, Name:«{name}» is not found");
            }
        }

        private static void CheckSession()
        {
#if UNITY_EDITOR
            if (Dispatcher.Instance == null) return;

            if (Dispatcher.Instance.SessionID == _sessionID) return;
            _signals.Clear();
            _sessionID = Dispatcher.Instance.SessionID;
#endif
        }

        private static void GetData(string name, out SignalData<T1, T2> signalData)
        {
            if (_signals.TryGetValue(name, out signalData)) return;
            signalData = new SignalData<T1, T2>(name);
            _signals.Add(name, signalData);
        }

        #endregion
    }

    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    [SuppressMessage("ReSharper", "StaticMemberInGenericType")]
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
    internal class Signal<T1, T2, T3>
    {
        #region ================================ FIELDS

        private static readonly Dictionary<string, SignalData<T1, T2, T3>> _signals = new Dictionary<string, SignalData<T1, T2, T3>>();
        private static Guid _sessionID;

        #endregion

        #region ================================ METHODS

        internal static void Fire(string name, T1 data1, T2 data2, T3 data3)
        {
            CheckSession();
            GetData(name, out var signal);

            if (signal.HasListeners == false)
            {
                if (DW4.AppConfig.LogDispatcher)
                    DW4.LogError($"[Signal.Fire]: Signal, Name:«{name}» has no listeners. Unable to fire signal");
                return;
            }

            if (DW4.AppConfig.LogDispatcher)
                DW4.Log($"[Signal.Fire]: Signal, Name:«{name}» is fired");

            signal.Lock();
            signal.Invoke(data1, data2, data3);
            signal.Unlock();
        }

        internal static void Subscribe(string name, Action<T1, T2, T3> callback, int order)
        {
            CheckSession();

            if (_signals.TryGetValue(name, out var signalData) == false)
            {
                signalData = new SignalData<T1, T2, T3>(name);
                _signals.Add(name, signalData);
            }

            if (DW4.AppConfig.LogDispatcher)
                DW4.Log($"[Signal.Subscribe]: Listener have been added to the signals, Name:«{name}»");

            signalData.Subscribe(callback, order);
        }

        internal static void Unsubscribe(string name, Action<T1, T2, T3> callback)
        {
            if (_signals.TryGetValue(name, out var signalData))
            {
                if (DW4.AppConfig.LogDispatcher)
                    DW4.Log($"[Signal.Unsubscribe]: Listener have been removed from the signals, Name:«{name}»");

                signalData.Unsubscribe(callback);
            }
            else
            {
                if (DW4.AppConfig.LogDispatcher)
                    DW4.LogError($"[Signal.Unsubscribe]: Any signal, Name:«{name}» is not found");
            }
        }

        private static void CheckSession()
        {
#if UNITY_EDITOR
            if (Dispatcher.Instance == null) return;

            if (Dispatcher.Instance.SessionID == _sessionID) return;
            _signals.Clear();
            _sessionID = Dispatcher.Instance.SessionID;
#endif
        }

        private static void GetData(string name, out SignalData<T1, T2, T3> signalData)
        {
            if (_signals.TryGetValue(name, out signalData)) return;
            signalData = new SignalData<T1, T2, T3>(name);
            _signals.Add(name, signalData);
        }

        #endregion
    }

    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    [SuppressMessage("ReSharper", "StaticMemberInGenericType")]
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
    internal class Signal<T1, T2, T3, T4>
    {
        #region ================================ FIELDS

        private static readonly Dictionary<string, SignalData<T1, T2, T3, T4>> _signals = new Dictionary<string, SignalData<T1, T2, T3, T4>>();
        private static Guid _sessionID;

        #endregion

        #region ================================ METHODS

        internal static void Fire(string name, T1 data1, T2 data2, T3 data3, T4 data4)
        {
            CheckSession();
            GetData(name, out var signal);

            if (signal.HasListeners == false)
            {
                if (DW4.AppConfig.LogDispatcher)
                    DW4.LogError($"[Signal.Fire]: Signal, Name:«{name}» has no listeners. Unable to fire signal");
                return;
            }

            if (DW4.AppConfig.LogDispatcher)
                DW4.Log($"[Signal.Fire]: Signal, Name:«{name}» is fired");

            signal.Lock = true;
            signal.Invoke(data1, data2, data3, data4);
            signal.Lock = false;
        }

        internal static void Subscribe(string name, Action<T1, T2, T3, T4> callback, int order)
        {
            CheckSession();

            if (_signals.TryGetValue(name, out var signalData) == false)
            {
                signalData = new SignalData<T1, T2, T3, T4>(name);
                _signals.Add(name, signalData);
            }

            if (DW4.AppConfig.LogDispatcher)
                DW4.Log($"[Signal.Subscribe]: Listener have been added to the signals, Name:«{name}»");

            signalData.Subscribe(callback, order);
        }

        internal static void Unsubscribe(string name, Action<T1, T2, T3, T4> callback)
        {
            if (_signals.TryGetValue(name, out var signalData))
            {
                if (DW4.AppConfig.LogDispatcher)
                    DW4.Log($"[Signal.Unsubscribe]: Listener have been removed from the signals, Name:«{name}»");

                signalData.Unsubscribe(callback);
            }
            else
            {
                if (DW4.AppConfig.LogDispatcher)
                    DW4.LogError($"[Signal.Unsubscribe]: Any signal, Name:«{name}» is not found");
            }
        }

        private static void CheckSession()
        {
#if UNITY_EDITOR
            if (Dispatcher.Instance == null) return;

            if (Dispatcher.Instance.SessionID == _sessionID) return;
            _signals.Clear();
            _sessionID = Dispatcher.Instance.SessionID;
#endif
        }

        private static void GetData(string name, out SignalData<T1, T2, T3, T4> signalData)
        {
            if (_signals.TryGetValue(name, out signalData)) return;
            signalData = new SignalData<T1, T2, T3, T4>(name);
            _signals.Add(name, signalData);
        }

        #endregion
    }

    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    [SuppressMessage("ReSharper", "StaticMemberInGenericType")]
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
    internal class Signal<T1, T2, T3, T4, T5>
    {
        #region ================================ FIELDS

        private static readonly Dictionary<string, SignalData<T1, T2, T3, T4, T5>> _signals = new Dictionary<string, SignalData<T1, T2, T3, T4, T5>>();
        private static Guid _sessionID;

        #endregion

        #region ================================ METHODS

        internal static void Fire(string name, T1 data1, T2 data2, T3 data3, T4 data4, T5 data5)
        {
            CheckSession();
            GetData(name, out var signal);

            if (signal.HasListeners == false)
            {
                if (DW4.AppConfig.LogDispatcher)
                    DW4.LogError($"[Signal.Fire]: Signal, Name:«{name}» has no listeners. Unable to fire signal");
                return;
            }

            if (DW4.AppConfig.LogDispatcher)
                DW4.Log($"[Signal.Fire]: Signal, Name:«{name}» is fired");

            signal.Lock = true;
            signal.Invoke(data1, data2, data3, data4, data5);
            signal.Lock = false;
        }

        internal static void Subscribe(string name, Action<T1, T2, T3, T4, T5> callback, int order)
        {
            CheckSession();

            if (_signals.TryGetValue(name, out var signalData) == false)
            {
                signalData = new SignalData<T1, T2, T3, T4, T5>(name);
                _signals.Add(name, signalData);
            }

            if (DW4.AppConfig.LogDispatcher)
                DW4.Log($"[Signal.Subscribe]: Listener have been added to the signals, Name:«{name}»");

            signalData.Subscribe(callback, order);
        }

        internal static void Unsubscribe(string name, Action<T1, T2, T3, T4, T5> callback)
        {
            if (_signals.TryGetValue(name, out var signalData))
            {
                if (DW4.AppConfig.LogDispatcher)
                    DW4.Log($"[Signal.Unsubscribe]: Listener have been removed from the signals, Name:«{name}»");

                signalData.Unsubscribe(callback);
            }
            else
            {
                if (DW4.AppConfig.LogDispatcher)
                    DW4.LogError($"[Signal.Unsubscribe]: Any signal, Name:«{name}» is not found");
            }
        }

        private static void CheckSession()
        {
#if UNITY_EDITOR
            if (Dispatcher.Instance == null) return;

            if (Dispatcher.Instance.SessionID == _sessionID) return;
            _signals.Clear();
            _sessionID = Dispatcher.Instance.SessionID;
#endif
        }

        private static void GetData(string name, out SignalData<T1, T2, T3, T4, T5> signalData)
        {
            if (_signals.TryGetValue(name, out signalData)) return;
            signalData = new SignalData<T1, T2, T3, T4, T5>(name);
            _signals.Add(name, signalData);
        }

        #endregion
    }

    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    [SuppressMessage("ReSharper", "StaticMemberInGenericType")]
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
    internal class Signal<T1, T2, T3, T4, T5, T6>
    {
        #region ================================ FIELDS

        private static readonly Dictionary<string, SignalData<T1, T2, T3, T4, T5, T6>> _signals = new Dictionary<string, SignalData<T1, T2, T3, T4, T5, T6>>();
        private static Guid _sessionID;

        #endregion

        #region ================================ METHODS

        internal static void Fire(string name, T1 data1, T2 data2, T3 data3, T4 data4, T5 data5, T6 data6)
        {
            CheckSession();
            GetData(name, out var signal);

            if (signal.HasListeners == false)
            {
                if (DW4.AppConfig.LogDispatcher)
                    DW4.LogError($"[Signal.Fire]: Signal, Name:«{name}» has no listeners. Unable to fire signal");
                return;
            }

            if (DW4.AppConfig.LogDispatcher)
                DW4.Log($"[Signal.Fire]: Signal, Name:«{name}» is fired");

            signal.Lock = true;
            signal.Invoke(data1, data2, data3, data4, data5, data6);
            signal.Lock = false;
        }

        internal static void Subscribe(string name, Action<T1, T2, T3, T4, T5, T6> callback, int order)
        {
            CheckSession();

            if (_signals.TryGetValue(name, out var signalData) == false)
            {
                signalData = new SignalData<T1, T2, T3, T4, T5, T6>(name);
                _signals.Add(name, signalData);
            }

            if (DW4.AppConfig.LogDispatcher)
                DW4.Log($"[Signal.Subscribe]: Listener have been added to the signals, Name:«{name}»");

            signalData.Subscribe(callback, order);
        }

        internal static void Unsubscribe(string name, Action<T1, T2, T3, T4, T5, T6> callback)
        {
            if (_signals.TryGetValue(name, out var signalData))
            {
                if (DW4.AppConfig.LogDispatcher)
                    DW4.Log($"[Signal.Unsubscribe]: Listener have been removed from the signals, Name:«{name}»");

                signalData.Unsubscribe(callback);
            }
            else
            {
                if (DW4.AppConfig.LogDispatcher)
                    DW4.LogError($"[Signal.Unsubscribe]: Any signal, Name:«{name}» is not found");
            }
        }

        private static void CheckSession()
        {
#if UNITY_EDITOR
            if (Dispatcher.Instance == null) return;

            if (Dispatcher.Instance.SessionID == _sessionID) return;
            _signals.Clear();
            _sessionID = Dispatcher.Instance.SessionID;
#endif
        }

        private static void GetData(string name, out SignalData<T1, T2, T3, T4, T5, T6> signalData)
        {
            if (_signals.TryGetValue(name, out signalData)) return;
            signalData = new SignalData<T1, T2, T3, T4, T5, T6>(name);
            _signals.Add(name, signalData);
        }

        #endregion
    }

    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    [SuppressMessage("ReSharper", "StaticMemberInGenericType")]
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
    internal class Signal<T1, T2, T3, T4, T5, T6, T7>
    {
        #region ================================ FIELDS

        private static readonly Dictionary<string, SignalData<T1, T2, T3, T4, T5, T6, T7>> _signals = new Dictionary<string, SignalData<T1, T2, T3, T4, T5, T6, T7>>();
        private static Guid _sessionID;

        #endregion

        #region ================================ METHODS

        internal static void Fire(string name, T1 data1, T2 data2, T3 data3, T4 data4, T5 data5, T6 data6, T7 data7)
        {
            CheckSession();
            GetData(name, out var signal);

            if (signal.HasListeners == false)
            {
                if (DW4.AppConfig.LogDispatcher)
                    DW4.LogError($"[Signal.Fire]: Signal, Name:«{name}» has no listeners. Unable to fire signal");
                return;
            }

            if (DW4.AppConfig.LogDispatcher)
                DW4.Log($"[Signal.Fire]: Signal, Name:«{name}» is fired");

            signal.Lock = true;
            signal.Invoke(data1, data2, data3, data4, data5, data6, data7);
            signal.Lock = false;
        }

        internal static void Subscribe(string name, Action<T1, T2, T3, T4, T5, T6, T7> callback, int order)
        {
            CheckSession();

            if (_signals.TryGetValue(name, out var signalData) == false)
            {
                signalData = new SignalData<T1, T2, T3, T4, T5, T6, T7>(name);
                _signals.Add(name, signalData);
            }

            if (DW4.AppConfig.LogDispatcher)
                DW4.Log($"[Signal.Subscribe]: Listener have been added to the signals, Name:«{name}»");

            signalData.Subscribe(callback, order);
        }

        internal static void Unsubscribe(string name, Action<T1, T2, T3, T4, T5, T6, T7> callback)
        {
            if (_signals.TryGetValue(name, out var signalData))
            {
                if (DW4.AppConfig.LogDispatcher)
                    DW4.Log($"[Signal.Unsubscribe]: Listener have been removed from the signals, Name:«{name}»");

                signalData.Unsubscribe(callback);
            }
            else
            {
                if (DW4.AppConfig.LogDispatcher)
                    DW4.LogError($"[Signal.Unsubscribe]: Any signal, Name:«{name}» is not found");
            }
        }

        private static void CheckSession()
        {
#if UNITY_EDITOR
            if (Dispatcher.Instance == null) return;

            if (Dispatcher.Instance.SessionID == _sessionID) return;
            _signals.Clear();
            _sessionID = Dispatcher.Instance.SessionID;
#endif
        }

        private static void GetData(string name, out SignalData<T1, T2, T3, T4, T5, T6, T7> signalData)
        {
            if (_signals.TryGetValue(name, out signalData)) return;
            signalData = new SignalData<T1, T2, T3, T4, T5, T6, T7>(name);
            _signals.Add(name, signalData);
        }

        #endregion
    }

    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    [SuppressMessage("ReSharper", "StaticMemberInGenericType")]
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
    internal class Signal<T1, T2, T3, T4, T5, T6, T7, T8>
    {
        #region ================================ FIELDS

        private static readonly Dictionary<string, SignalData<T1, T2, T3, T4, T5, T6, T7, T8>> _signals = new Dictionary<string, SignalData<T1, T2, T3, T4, T5, T6, T7, T8>>();
        private static Guid _sessionID;

        #endregion

        #region ================================ METHODS

        internal static void Fire(string name, T1 data1, T2 data2, T3 data3, T4 data4, T5 data5, T6 data6, T7 data7, T8 data8)
        {
            CheckSession();
            GetData(name, out var signal);

            if (signal.HasListeners == false)
            {
                if (DW4.AppConfig.LogDispatcher)
                    DW4.LogError($"[Signal.Fire]: Signal, Name:«{name}» has no listeners. Unable to fire signal");
                return;
            }

            if (DW4.AppConfig.LogDispatcher)
                DW4.Log($"[Signal.Fire]: Signal, Name:«{name}» is fired");

            signal.Lock = true;
            signal.Invoke(data1, data2, data3, data4, data5, data6, data7, data8);
            signal.Lock = false;
        }

        internal static void Subscribe(string name, Action<T1, T2, T3, T4, T5, T6, T7, T8> callback, int order)
        {
            CheckSession();

            if (_signals.TryGetValue(name, out var signalData) == false)
            {
                signalData = new SignalData<T1, T2, T3, T4, T5, T6, T7, T8>(name);
                _signals.Add(name, signalData);
            }

            if (DW4.AppConfig.LogDispatcher)
                DW4.Log($"[Signal.Subscribe]: Listener have been added to the signals, Name:«{name}»");

            signalData.Subscribe(callback, order);
        }

        internal static void Unsubscribe(string name, Action<T1, T2, T3, T4, T5, T6, T7, T8> callback)
        {
            if (_signals.TryGetValue(name, out var signalData))
            {
                if (DW4.AppConfig.LogDispatcher)
                    DW4.Log($"[Signal.Unsubscribe]: Listener have been removed from the signals, Name:«{name}»");

                signalData.Unsubscribe(callback);
            }
            else
            {
                if (DW4.AppConfig.LogDispatcher)
                    DW4.LogError($"[Signal.Unsubscribe]: Any signal, Name:«{name}» is not found");
            }
        }

        private static void CheckSession()
        {
#if UNITY_EDITOR
            if (Dispatcher.Instance == null) return;

            if (Dispatcher.Instance.SessionID == _sessionID) return;
            _signals.Clear();
            _sessionID = Dispatcher.Instance.SessionID;
#endif
        }

        private static void GetData(string name, out SignalData<T1, T2, T3, T4, T5, T6, T7, T8> signalData)
        {
            if (_signals.TryGetValue(name, out signalData)) return;
            signalData = new SignalData<T1, T2, T3, T4, T5, T6, T7, T8>(name);
            _signals.Add(name, signalData);
        }

        #endregion
    }

    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    [SuppressMessage("ReSharper", "StaticMemberInGenericType")]
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
    internal class Signal<T1, T2, T3, T4, T5, T6, T7, T8, T9>
    {
        #region ================================ FIELDS

        private static readonly Dictionary<string, SignalData<T1, T2, T3, T4, T5, T6, T7, T8, T9>> _signals = new Dictionary<string, SignalData<T1, T2, T3, T4, T5, T6, T7, T8, T9>>();
        private static Guid _sessionID;

        #endregion

        #region ================================ METHODS

        internal static void Fire(string name, T1 data1, T2 data2, T3 data3, T4 data4, T5 data5, T6 data6, T7 data7, T8 data8, T9 data9)
        {
            CheckSession();
            GetData(name, out var signal);

            if (signal.HasListeners == false)
            {
                if (DW4.AppConfig.LogDispatcher)
                    DW4.LogError($"[Signal.Fire]: Signal, Name:«{name}» has no listeners. Unable to fire signal");
                return;
            }

            if (DW4.AppConfig.LogDispatcher)
                DW4.Log($"[Signal.Fire]: Signal, Name:«{name}» is fired");

            signal.Lock = true;
            signal.Invoke(data1, data2, data3, data4, data5, data6, data7, data8, data9);
            signal.Lock = false;
        }

        internal static void Subscribe(string name, Action<T1, T2, T3, T4, T5, T6, T7, T8, T9> callback, int order)
        {
            CheckSession();

            if (_signals.TryGetValue(name, out var signalData) == false)
            {
                signalData = new SignalData<T1, T2, T3, T4, T5, T6, T7, T8, T9>(name);
                _signals.Add(name, signalData);
            }

            if (DW4.AppConfig.LogDispatcher)
                DW4.Log($"[Signal.Subscribe]: Listener have been added to the signals, Name:«{name}»");

            signalData.Subscribe(callback, order);
        }

        internal static void Unsubscribe(string name, Action<T1, T2, T3, T4, T5, T6, T7, T8, T9> callback)
        {
            if (_signals.TryGetValue(name, out var signalData))
            {
                if (DW4.AppConfig.LogDispatcher)
                    DW4.Log($"[Signal.Unsubscribe]: Listener have been removed from the signals, Name:«{name}»");

                signalData.Unsubscribe(callback);
            }
            else
            {
                if (DW4.AppConfig.LogDispatcher)
                    DW4.LogError($"[Signal.Unsubscribe]: Any signal, Name:«{name}» is not found");
            }
        }

        private static void CheckSession()
        {
#if UNITY_EDITOR
            if (Dispatcher.Instance == null) return;

            if (Dispatcher.Instance.SessionID == _sessionID) return;
            _signals.Clear();
            _sessionID = Dispatcher.Instance.SessionID;
#endif
        }

        private static void GetData(string name, out SignalData<T1, T2, T3, T4, T5, T6, T7, T8, T9> signalData)
        {
            if (_signals.TryGetValue(name, out signalData)) return;
            signalData = new SignalData<T1, T2, T3, T4, T5, T6, T7, T8, T9>(name);
            _signals.Add(name, signalData);
        }

        #endregion
    }

    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    [SuppressMessage("ReSharper", "StaticMemberInGenericType")]
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
    internal class Signal<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>
    {
        #region ================================ FIELDS

        private static readonly Dictionary<string, SignalData<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>> _signals = new Dictionary<string, SignalData<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>>();
        private static Guid _sessionID;

        #endregion

        #region ================================ METHODS

        internal static void Fire(string name, T1 data1, T2 data2, T3 data3, T4 data4, T5 data5, T6 data6, T7 data7, T8 data8, T9 data9, T10 data10)
        {
            CheckSession();
            GetData(name, out var signal);

            if (signal.HasListeners == false)
            {
                if (DW4.AppConfig.LogDispatcher)
                    DW4.LogError($"[Signal.Fire]: Signal, Name:«{name}» has no listeners. Unable to fire signal");
                return;
            }

            if (DW4.AppConfig.LogDispatcher)
                DW4.Log($"[Signal.Fire]: Signal, Name:«{name}» is fired");

            signal.Lock = true;
            signal.Invoke(data1, data2, data3, data4, data5, data6, data7, data8, data9, data10);
            signal.Lock = false;
        }

        internal static void Subscribe(string name, Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> callback, int order)
        {
            CheckSession();

            if (_signals.TryGetValue(name, out var signalData) == false)
            {
                signalData = new SignalData<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(name);
                _signals.Add(name, signalData);
            }

            if (DW4.AppConfig.LogDispatcher)
                DW4.Log($"[Signal.Subscribe]: Listener have been added to the signals, Name:«{name}»");

            signalData.Subscribe(callback, order);
        }

        internal static void Unsubscribe(string name, Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> callback)
        {
            if (_signals.TryGetValue(name, out var signalData))
            {
                if (DW4.AppConfig.LogDispatcher)
                    DW4.Log($"[Signal.Unsubscribe]: Listener have been removed from the signals, Name:«{name}»");

                signalData.Unsubscribe(callback);
            }
            else
            {
                if (DW4.AppConfig.LogDispatcher)
                    DW4.LogError($"[Signal.Unsubscribe]: Any signal, Name:«{name}» is not found");
            }
        }

        private static void CheckSession()
        {
#if UNITY_EDITOR
            if (Dispatcher.Instance == null) return;

            if (Dispatcher.Instance.SessionID == _sessionID) return;
            _signals.Clear();
            _sessionID = Dispatcher.Instance.SessionID;
#endif
        }

        private static void GetData(string name, out SignalData<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> signalData)
        {
            if (_signals.TryGetValue(name, out signalData)) return;
            signalData = new SignalData<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(name);
            _signals.Add(name, signalData);
        }

        #endregion
    }
}