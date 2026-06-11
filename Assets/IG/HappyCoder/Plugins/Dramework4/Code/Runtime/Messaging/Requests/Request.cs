using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

using IG.HappyCoder.Plugins.Dramework4.Runtime.Core;
using IG.HappyCoder.Plugins.Dramework4.Runtime.Tools;

using Unity.IL2CPP.CompilerServices;


namespace IG.HappyCoder.Plugins.Dramework4.Runtime.Messaging.Requests
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    [SuppressMessage("ReSharper", "StaticMemberInGenericType")]
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
    internal class Request<TOut>
    {
        #region ================================ FIELDS

        private static readonly Dictionary<string, RequestData<TOut>> _requests = new Dictionary<string, RequestData<TOut>>();
        private static Guid _sessionID;

        #endregion

        #region ================================ METHODS

        internal static TOut Fire(string name)
        {
            CheckSession();
            Add(name, out var request);

            if (request.Listener == null)
            {
                if (DW4.AppConfig.LogDispatcher)
                    DW4.LogError($"[Request.Fire]: Request Name:«{name}» has no listener. Unable to fire request");

                return default;
            }

            if (DW4.AppConfig.LogDispatcher)
                DW4.Log($"[Request.Fire]: Request, Name:«{name}» is fired");

            return request.Listener.Invoke();
        }

        internal static void Subscribe(string name, Func<TOut> callback)
        {
            CheckSession();
            Add(name, out var request);

            if (DW4.AppConfig.LogDispatcher)
                DW4.Log($"[Request.Subscribe]: Listener have been added to the request, Name:«{name}»");

            request.Listener = callback;
        }

        internal static void Unsubscribe(string name, Func<TOut> callback)
        {
            if (_requests.TryGetValue(name, out var request))
            {
                if (DW4.AppConfig.LogDispatcher)
                    DW4.Log($"[Request.Unsubscribe]: Listener have been removed from the request, Name:«{name}»");

                request.Listener -= callback;
            }
            else
            {
                if (DW4.AppConfig.LogDispatcher)
                    DW4.LogError($"[Request.Unsubscribe]: Request, Name:«{name}» is not found");
            }
        }

        private static void Add(string name, out RequestData<TOut> request)
        {
            if (_requests.TryGetValue(name, out request)) return;
            request = new RequestData<TOut>();
            _requests.Add(name, request);
        }

        private static void CheckSession()
        {
#if UNITY_EDITOR
            if (Dispatcher.Instance == null) return;

            if (Dispatcher.Instance.SessionID == _sessionID) return;
            _requests.Clear();
            _sessionID = Dispatcher.Instance.SessionID;
#endif
        }

        #endregion
    }

    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    [SuppressMessage("ReSharper", "StaticMemberInGenericType")]
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
    internal class Request<TIn, TOut>
    {
        #region ================================ FIELDS

        private static readonly Dictionary<string, RequestData<TIn, TOut>> _requests = new Dictionary<string, RequestData<TIn, TOut>>();
        private static Guid _sessionID;

        #endregion

        #region ================================ METHODS

        internal static TOut Fire(string name, TIn data)
        {
            CheckSession();
            Add(name, out var request);

            if (request.Listener == null)
            {
                if (DW4.AppConfig.LogDispatcher)
                    DW4.LogError($"[Request.Fire]: Request Name:«{name}» has no listener. Unable to fire request");

                return default;
            }

            if (DW4.AppConfig.LogDispatcher)
                DW4.Log($"[Request.Fire]: Request, Name:«{name}» is fired");

            return request.Listener.Invoke(data);
        }

        internal static void Subscribe(string name, Func<TIn, TOut> callback)
        {
            CheckSession();
            Add(name, out var request);

            if (DW4.AppConfig.LogDispatcher)
                DW4.Log($"[Request.Subscribe]: Listener have been added to the request, Name:«{name}»");

            request.Listener = callback;
        }

        internal static void Unsubscribe(string name, Func<TIn, TOut> callback)
        {
            if (_requests.TryGetValue(name, out var request))
            {
                if (DW4.AppConfig.LogDispatcher)
                    DW4.Log($"[Request.Unsubscribe]: Listener have been removed from the request, Name:«{name}»");

                request.Listener -= callback;
            }
            else
            {
                if (DW4.AppConfig.LogDispatcher)
                    DW4.LogError($"[Request.Unsubscribe]: Request, Name:«{name}» is not found");
            }
        }

        private static void Add(string name, out RequestData<TIn, TOut> request)
        {
            if (_requests.TryGetValue(name, out request)) return;
            request = new RequestData<TIn, TOut>();
            _requests.Add(name, request);
        }

        private static void CheckSession()
        {
#if UNITY_EDITOR
            if (Dispatcher.Instance == null) return;

            if (Dispatcher.Instance.SessionID == _sessionID) return;
            _requests.Clear();
            _sessionID = Dispatcher.Instance.SessionID;
#endif
        }

        #endregion
    }

    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    [SuppressMessage("ReSharper", "StaticMemberInGenericType")]
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
    internal class Request<TIn1, TIn2, TOut>
    {
        #region ================================ FIELDS

        private static readonly Dictionary<string, RequestData<TIn1, TIn2, TOut>> _requests = new Dictionary<string, RequestData<TIn1, TIn2, TOut>>();
        private static Guid _sessionID;

        #endregion

        #region ================================ METHODS

        internal static TOut Fire(string name, TIn1 data1, TIn2 data2)
        {
            CheckSession();
            Add(name, out var request);

            if (request.Listener == null)
            {
                if (DW4.AppConfig.LogDispatcher)
                    DW4.LogError($"[Request.Fire]: Request Name:«{name}» has no listener. Unable to fire request");

                return default;
            }

            if (DW4.AppConfig.LogDispatcher)
                DW4.Log($"[Request.Fire]: Request, Name:«{name}» is fired");

            return request.Listener.Invoke(data1, data2);
        }

        internal static void Subscribe(string name, Func<TIn1, TIn2, TOut> callback)
        {
            CheckSession();
            Add(name, out var request);

            if (DW4.AppConfig.LogDispatcher)
                DW4.Log($"[Request.Subscribe]: Listener have been added to the request, Name:«{name}»");

            request.Listener = callback;
        }

        internal static void Unsubscribe(string name, Func<TIn1, TIn2, TOut> callback)
        {
            if (_requests.TryGetValue(name, out var request))
            {
                if (DW4.AppConfig.LogDispatcher)
                    DW4.Log($"[Request.Unsubscribe]: Listener have been removed from the request, Name:«{name}»");

                request.Listener -= callback;
            }
            else
            {
                if (DW4.AppConfig.LogDispatcher)
                    DW4.LogError($"[Request.Unsubscribe]: Request, Name:«{name}» is not found");
            }
        }

        private static void Add(string name, out RequestData<TIn1, TIn2, TOut> request)
        {
            if (_requests.TryGetValue(name, out request)) return;
            request = new RequestData<TIn1, TIn2, TOut>();
            _requests.Add(name, request);
        }

        private static void CheckSession()
        {
#if UNITY_EDITOR
            if (Dispatcher.Instance == null) return;

            if (Dispatcher.Instance.SessionID == _sessionID) return;
            _requests.Clear();
            _sessionID = Dispatcher.Instance.SessionID;
#endif
        }

        #endregion
    }

    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    [SuppressMessage("ReSharper", "StaticMemberInGenericType")]
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
    internal class Request<TIn1, TIn2, TIn3, TOut>
    {
        #region ================================ FIELDS

        private static readonly Dictionary<string, RequestData<TIn1, TIn2, TIn3, TOut>> _requests = new Dictionary<string, RequestData<TIn1, TIn2, TIn3, TOut>>();
        private static Guid _sessionID;

        #endregion

        #region ================================ METHODS

        internal static TOut Fire(string name, TIn1 data1, TIn2 data2, TIn3 data3)
        {
            CheckSession();
            Add(name, out var request);

            if (request.Listener == null)
            {
                if (DW4.AppConfig.LogDispatcher)
                    DW4.LogError($"[Request.Fire]: Request Name:«{name}» has no listener. Unable to fire request");

                return default;
            }

            if (DW4.AppConfig.LogDispatcher)
                DW4.Log($"[Request.Fire]: Request, Name:«{name}» is fired");

            return request.Listener.Invoke(data1, data2, data3);
        }

        internal static void Subscribe(string name, Func<TIn1, TIn2, TIn3, TOut> callback)
        {
            CheckSession();
            Add(name, out var request);

            if (DW4.AppConfig.LogDispatcher)
                DW4.Log($"[Request.Subscribe]: Listener have been added to the request, Name:«{name}»");

            request.Listener = callback;
        }

        internal static void Unsubscribe(string name, Func<TIn1, TIn2, TIn3, TOut> callback)
        {
            if (_requests.TryGetValue(name, out var request))
            {
                if (DW4.AppConfig.LogDispatcher)
                    DW4.Log($"[Request.Unsubscribe]: Listener have been removed from the request, Name:«{name}»");

                request.Listener -= callback;
            }
            else
            {
                if (DW4.AppConfig.LogDispatcher)
                    DW4.LogError($"[Request.Unsubscribe]: Request, Name:«{name}» is not found");
            }
        }

        private static void Add(string name, out RequestData<TIn1, TIn2, TIn3, TOut> request)
        {
            if (_requests.TryGetValue(name, out request)) return;
            request = new RequestData<TIn1, TIn2, TIn3, TOut>();
            _requests.Add(name, request);
        }

        private static void CheckSession()
        {
#if UNITY_EDITOR
            if (Dispatcher.Instance == null) return;

            if (Dispatcher.Instance.SessionID == _sessionID) return;
            _requests.Clear();
            _sessionID = Dispatcher.Instance.SessionID;
#endif
        }

        #endregion
    }

    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    [SuppressMessage("ReSharper", "StaticMemberInGenericType")]
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
    internal class Request<TIn1, TIn2, TIn3, TIn4, TOut>
    {
        #region ================================ FIELDS

        private static readonly Dictionary<string, RequestData<TIn1, TIn2, TIn3, TIn4, TOut>> _requests = new Dictionary<string, RequestData<TIn1, TIn2, TIn3, TIn4, TOut>>();
        private static Guid _sessionID;

        #endregion

        #region ================================ METHODS

        internal static TOut Fire(string name, TIn1 data1, TIn2 data2, TIn3 data3, TIn4 data4)
        {
            CheckSession();
            Add(name, out var request);

            if (request.Listener == null)
            {
                if (DW4.AppConfig.LogDispatcher)
                    DW4.LogError($"[Request.Fire]: Request Name:«{name}» has no listener. Unable to fire request");

                return default;
            }

            if (DW4.AppConfig.LogDispatcher)
                DW4.Log($"[Request.Fire]: Request, Name:«{name}» is fired");

            return request.Listener.Invoke(data1, data2, data3, data4);
        }

        internal static void Subscribe(string name, Func<TIn1, TIn2, TIn3, TIn4, TOut> callback)
        {
            CheckSession();
            Add(name, out var request);

            if (DW4.AppConfig.LogDispatcher)
                DW4.Log($"[Request.Subscribe]: Listener have been added to the request, Name:«{name}»");

            request.Listener = callback;
        }

        internal static void Unsubscribe(string name, Func<TIn1, TIn2, TIn3, TIn4, TOut> callback)
        {
            if (_requests.TryGetValue(name, out var request))
            {
                if (DW4.AppConfig.LogDispatcher)
                    DW4.Log($"[Request.Unsubscribe]: Listener have been removed from the request, Name:«{name}»");

                request.Listener -= callback;
            }
            else
            {
                if (DW4.AppConfig.LogDispatcher)
                    DW4.LogError($"[Request.Unsubscribe]: Request, Name:«{name}» is not found");
            }
        }

        private static void Add(string name, out RequestData<TIn1, TIn2, TIn3, TIn4, TOut> request)
        {
            if (_requests.TryGetValue(name, out request)) return;
            request = new RequestData<TIn1, TIn2, TIn3, TIn4, TOut>();
            _requests.Add(name, request);
        }

        private static void CheckSession()
        {
#if UNITY_EDITOR
            if (Dispatcher.Instance == null) return;

            if (Dispatcher.Instance.SessionID == _sessionID) return;
            _requests.Clear();
            _sessionID = Dispatcher.Instance.SessionID;
#endif
        }

        #endregion
    }

    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    [SuppressMessage("ReSharper", "StaticMemberInGenericType")]
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
    internal class Request<TIn1, TIn2, TIn3, TIn4, TIn5, TOut>
    {
        #region ================================ FIELDS

        private static readonly Dictionary<string, RequestData<TIn1, TIn2, TIn3, TIn4, TIn5, TOut>> _requests = new Dictionary<string, RequestData<TIn1, TIn2, TIn3, TIn4, TIn5, TOut>>();
        private static Guid _sessionID;

        #endregion

        #region ================================ METHODS

        internal static TOut Fire(string name, TIn1 data1, TIn2 data2, TIn3 data3, TIn4 data4, TIn5 data5)
        {
            CheckSession();
            Add(name, out var request);

            if (request.Listener == null)
            {
                if (DW4.AppConfig.LogDispatcher)
                    DW4.LogError($"[Request.Fire]: Request Name:«{name}» has no listener. Unable to fire request");

                return default;
            }

            if (DW4.AppConfig.LogDispatcher)
                DW4.Log($"[Request.Fire]: Request, Name:«{name}» is fired");

            return request.Listener.Invoke(data1, data2, data3, data4, data5);
        }

        internal static void Subscribe(string name, Func<TIn1, TIn2, TIn3, TIn4, TIn5, TOut> callback)
        {
            CheckSession();
            Add(name, out var request);

            if (DW4.AppConfig.LogDispatcher)
                DW4.Log($"[Request.Subscribe]: Listener have been added to the request, Name:«{name}»");

            request.Listener = callback;
        }

        internal static void Unsubscribe(string name, Func<TIn1, TIn2, TIn3, TIn4, TIn5, TOut> callback)
        {
            if (_requests.TryGetValue(name, out var request))
            {
                if (DW4.AppConfig.LogDispatcher)
                    DW4.Log($"[Request.Unsubscribe]: Listener have been removed from the request, Name:«{name}»");

                request.Listener -= callback;
            }
            else
            {
                if (DW4.AppConfig.LogDispatcher)
                    DW4.LogError($"[Request.Unsubscribe]: Request, Name:«{name}» is not found");
            }
        }

        private static void Add(string name, out RequestData<TIn1, TIn2, TIn3, TIn4, TIn5, TOut> request)
        {
            if (_requests.TryGetValue(name, out request)) return;
            request = new RequestData<TIn1, TIn2, TIn3, TIn4, TIn5, TOut>();
            _requests.Add(name, request);
        }

        private static void CheckSession()
        {
#if UNITY_EDITOR
            if (Dispatcher.Instance == null) return;

            if (Dispatcher.Instance.SessionID == _sessionID) return;
            _requests.Clear();
            _sessionID = Dispatcher.Instance.SessionID;
#endif
        }

        #endregion
    }

    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    [SuppressMessage("ReSharper", "StaticMemberInGenericType")]
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
    internal class Request<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, TOut>
    {
        #region ================================ FIELDS

        private static readonly Dictionary<string, RequestData<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, TOut>> _requests = new Dictionary<string, RequestData<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, TOut>>();
        private static Guid _sessionID;

        #endregion

        #region ================================ METHODS

        internal static TOut Fire(string name, TIn1 data1, TIn2 data2, TIn3 data3, TIn4 data4, TIn5 data5, TIn6 data6)
        {
            CheckSession();
            Add(name, out var request);

            if (request.Listener == null)
            {
                if (DW4.AppConfig.LogDispatcher)
                    DW4.LogError($"[Request.Fire]: Request Name:«{name}» has no listener. Unable to fire request");

                return default;
            }

            if (DW4.AppConfig.LogDispatcher)
                DW4.Log($"[Request.Fire]: Request, Name:«{name}» is fired");

            return request.Listener.Invoke(data1, data2, data3, data4, data5, data6);
        }

        internal static void Subscribe(string name, Func<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, TOut> callback)
        {
            CheckSession();
            Add(name, out var request);

            if (DW4.AppConfig.LogDispatcher)
                DW4.Log($"[Request.Subscribe]: Listener have been added to the request, Name:«{name}»");

            request.Listener = callback;
        }

        internal static void Unsubscribe(string name, Func<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, TOut> callback)
        {
            if (_requests.TryGetValue(name, out var request))
            {
                if (DW4.AppConfig.LogDispatcher)
                    DW4.Log($"[Request.Unsubscribe]: Listener have been removed from the request, Name:«{name}»");

                request.Listener -= callback;
            }
            else
            {
                if (DW4.AppConfig.LogDispatcher)
                    DW4.LogError($"[Request.Unsubscribe]: Request, Name:«{name}» is not found");
            }
        }

        private static void Add(string name, out RequestData<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, TOut> request)
        {
            if (_requests.TryGetValue(name, out request)) return;
            request = new RequestData<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, TOut>();
            _requests.Add(name, request);
        }

        private static void CheckSession()
        {
#if UNITY_EDITOR
            if (Dispatcher.Instance == null) return;

            if (Dispatcher.Instance.SessionID == _sessionID) return;
            _requests.Clear();
            _sessionID = Dispatcher.Instance.SessionID;
#endif
        }

        #endregion
    }

    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    [SuppressMessage("ReSharper", "StaticMemberInGenericType")]
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
    internal class Request<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, TIn7, TOut>
    {
        #region ================================ FIELDS

        private static readonly Dictionary<string, RequestData<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, TIn7, TOut>> _requests =
            new Dictionary<string, RequestData<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, TIn7, TOut>>();

        private static Guid _sessionID;

        #endregion

        #region ================================ METHODS

        internal static TOut Fire(string name, TIn1 data1, TIn2 data2, TIn3 data3, TIn4 data4, TIn5 data5, TIn6 data6, TIn7 data7)
        {
            CheckSession();
            Add(name, out var request);

            if (request.Listener == null)
            {
                if (DW4.AppConfig.LogDispatcher)
                    DW4.LogError($"[Request.Fire]: Request Name:«{name}» has no listener. Unable to fire request");

                return default;
            }

            if (DW4.AppConfig.LogDispatcher)
                DW4.Log($"[Request.Fire]: Request, Name:«{name}» is fired");

            return request.Listener.Invoke(data1, data2, data3, data4, data5, data6, data7);
        }

        internal static void Subscribe(string name, Func<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, TIn7, TOut> callback)
        {
            CheckSession();
            Add(name, out var request);

            if (DW4.AppConfig.LogDispatcher)
                DW4.Log($"[Request.Subscribe]: Listener have been added to the request, Name:«{name}»");

            request.Listener = callback;
        }

        internal static void Unsubscribe(string name, Func<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, TIn7, TOut> callback)
        {
            if (_requests.TryGetValue(name, out var request))
            {
                if (DW4.AppConfig.LogDispatcher)
                    DW4.Log($"[Request.Unsubscribe]: Listener have been removed from the request, Name:«{name}»");

                request.Listener -= callback;
            }
            else
            {
                if (DW4.AppConfig.LogDispatcher)
                    DW4.LogError($"[Request.Unsubscribe]: Request, Name:«{name}» is not found");
            }
        }

        private static void Add(string name, out RequestData<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, TIn7, TOut> request)
        {
            if (_requests.TryGetValue(name, out request)) return;
            request = new RequestData<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, TIn7, TOut>();
            _requests.Add(name, request);
        }

        private static void CheckSession()
        {
#if UNITY_EDITOR
            if (Dispatcher.Instance == null) return;

            if (Dispatcher.Instance.SessionID == _sessionID) return;
            _requests.Clear();
            _sessionID = Dispatcher.Instance.SessionID;
#endif
        }

        #endregion
    }

    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    [SuppressMessage("ReSharper", "StaticMemberInGenericType")]
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
    internal class Request<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, TIn7, TIn8, TOut>
    {
        #region ================================ FIELDS

        private static readonly Dictionary<string, RequestData<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, TIn7, TIn8, TOut>> _requests =
            new Dictionary<string, RequestData<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, TIn7, TIn8, TOut>>();

        private static Guid _sessionID;

        #endregion

        #region ================================ METHODS

        internal static TOut Fire(string name, TIn1 data1, TIn2 data2, TIn3 data3, TIn4 data4, TIn5 data5, TIn6 data6, TIn7 data7, TIn8 data8)
        {
            CheckSession();
            Add(name, out var request);

            if (request.Listener == null)
            {
                if (DW4.AppConfig.LogDispatcher)
                    DW4.LogError($"[Request.Fire]: Request Name:«{name}» has no listener. Unable to fire request");

                return default;
            }

            if (DW4.AppConfig.LogDispatcher)
                DW4.Log($"[Request.Fire]: Request, Name:«{name}» is fired");

            return request.Listener.Invoke(data1, data2, data3, data4, data5, data6, data7, data8);
        }

        internal static void Subscribe(string name, Func<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, TIn7, TIn8, TOut> callback)
        {
            CheckSession();
            Add(name, out var request);

            if (DW4.AppConfig.LogDispatcher)
                DW4.Log($"[Request.Subscribe]: Listener have been added to the request, Name:«{name}»");

            request.Listener = callback;
        }

        internal static void Unsubscribe(string name, Func<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, TIn7, TIn8, TOut> callback)
        {
            if (_requests.TryGetValue(name, out var request))
            {
                if (DW4.AppConfig.LogDispatcher)
                    DW4.Log($"[Request.Unsubscribe]: Listener have been removed from the request, Name:«{name}»");

                request.Listener -= callback;
            }
            else
            {
                if (DW4.AppConfig.LogDispatcher)
                    DW4.LogError($"[Request.Unsubscribe]: Request, Name:«{name}» is not found");
            }
        }

        private static void Add(string name, out RequestData<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, TIn7, TIn8, TOut> request)
        {
            if (_requests.TryGetValue(name, out request)) return;
            request = new RequestData<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, TIn7, TIn8, TOut>();
            _requests.Add(name, request);
        }

        private static void CheckSession()
        {
#if UNITY_EDITOR
            if (Dispatcher.Instance == null) return;

            if (Dispatcher.Instance.SessionID == _sessionID) return;
            _requests.Clear();
            _sessionID = Dispatcher.Instance.SessionID;
#endif
        }

        #endregion
    }

    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    [SuppressMessage("ReSharper", "StaticMemberInGenericType")]
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
    internal class Request<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, TIn7, TIn8, TIn9, TOut>
    {
        #region ================================ FIELDS

        private static readonly Dictionary<string, RequestData<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, TIn7, TIn8, TIn9, TOut>> _requests =
            new Dictionary<string, RequestData<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, TIn7, TIn8, TIn9, TOut>>();

        private static Guid _sessionID;

        #endregion

        #region ================================ METHODS

        internal static TOut Fire(string name, TIn1 data1, TIn2 data2, TIn3 data3, TIn4 data4, TIn5 data5, TIn6 data6, TIn7 data7, TIn8 data8, TIn9 data9)
        {
            CheckSession();
            Add(name, out var request);

            if (request.Listener == null)
            {
                if (DW4.AppConfig.LogDispatcher)
                    DW4.LogError($"[Request.Fire]: Request Name:«{name}» has no listener. Unable to fire request");

                return default;
            }

            if (DW4.AppConfig.LogDispatcher)
                DW4.Log($"[Request.Fire]: Request, Name:«{name}» is fired");

            return request.Listener.Invoke(data1, data2, data3, data4, data5, data6, data7, data8, data9);
        }

        internal static void Subscribe(string name, Func<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, TIn7, TIn8, TIn9, TOut> callback)
        {
            CheckSession();
            Add(name, out var request);

            if (DW4.AppConfig.LogDispatcher)
                DW4.Log($"[Request.Subscribe]: Listener have been added to the request, Name:«{name}»");

            request.Listener = callback;
        }

        internal static void Unsubscribe(string name, Func<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, TIn7, TIn8, TIn9, TOut> callback)
        {
            if (_requests.TryGetValue(name, out var request))
            {
                if (DW4.AppConfig.LogDispatcher)
                    DW4.Log($"[Request.Unsubscribe]: Listener have been removed from the request, Name:«{name}»");

                request.Listener -= callback;
            }
            else
            {
                if (DW4.AppConfig.LogDispatcher)
                    DW4.LogError($"[Request.Unsubscribe]: Request, Name:«{name}» is not found");
            }
        }

        private static void Add(string name, out RequestData<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, TIn7, TIn8, TIn9, TOut> request)
        {
            if (_requests.TryGetValue(name, out request)) return;
            request = new RequestData<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, TIn7, TIn8, TIn9, TOut>();
            _requests.Add(name, request);
        }

        private static void CheckSession()
        {
#if UNITY_EDITOR
            if (Dispatcher.Instance == null) return;

            if (Dispatcher.Instance.SessionID == _sessionID) return;
            _requests.Clear();
            _sessionID = Dispatcher.Instance.SessionID;
#endif
        }

        #endregion
    }

    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    [SuppressMessage("ReSharper", "StaticMemberInGenericType")]
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
    internal class Request<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, TIn7, TIn8, TIn9, TIn10, TOut>
    {
        #region ================================ FIELDS

        private static readonly Dictionary<string, RequestData<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, TIn7, TIn8, TIn9, TIn10, TOut>> _requests =
            new Dictionary<string, RequestData<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, TIn7, TIn8, TIn9, TIn10, TOut>>();

        private static Guid _sessionID;

        #endregion

        #region ================================ METHODS

        internal static TOut Fire(string name, TIn1 data1, TIn2 data2, TIn3 data3, TIn4 data4, TIn5 data5, TIn6 data6, TIn7 data7, TIn8 data8, TIn9 data9, TIn10 data10)
        {
            CheckSession();
            Add(name, out var request);

            if (request.Listener == null)
            {
                if (DW4.AppConfig.LogDispatcher)
                    DW4.LogError($"[Request.Fire]: Request Name:«{name}» has no listener. Unable to fire request");

                return default;
            }

            if (DW4.AppConfig.LogDispatcher)
                DW4.Log($"[Request.Fire]: Request, Name:«{name}» is fired");

            return request.Listener.Invoke(data1, data2, data3, data4, data5, data6, data7, data8, data9, data10);
        }

        internal static void Subscribe(string name, Func<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, TIn7, TIn8, TIn9, TIn10, TOut> callback)
        {
            CheckSession();
            Add(name, out var request);

            if (DW4.AppConfig.LogDispatcher)
                DW4.Log($"[Request.Subscribe]: Listener have been added to the request, Name:«{name}»");

            request.Listener = callback;
        }

        internal static void Unsubscribe(string name, Func<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, TIn7, TIn8, TIn9, TIn10, TOut> callback)
        {
            if (_requests.TryGetValue(name, out var request))
            {
                if (DW4.AppConfig.LogDispatcher)
                    DW4.Log($"[Request.Unsubscribe]: Listener have been removed from the request, Name:«{name}»");

                request.Listener -= callback;
            }
            else
            {
                if (DW4.AppConfig.LogDispatcher)
                    DW4.LogError($"[Request.Unsubscribe]: Request, Name:«{name}» is not found");
            }
        }

        private static void Add(string name, out RequestData<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, TIn7, TIn8, TIn9, TIn10, TOut> request)
        {
            if (_requests.TryGetValue(name, out request)) return;
            request = new RequestData<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, TIn7, TIn8, TIn9, TIn10, TOut>();
            _requests.Add(name, request);
        }

        private static void CheckSession()
        {
#if UNITY_EDITOR
            if (Dispatcher.Instance == null) return;

            if (Dispatcher.Instance.SessionID == _sessionID) return;
            _requests.Clear();
            _sessionID = Dispatcher.Instance.SessionID;
#endif
        }

        #endregion
    }
}