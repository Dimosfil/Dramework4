using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;

using Cysharp.Threading.Tasks;

using IG.HappyCoder.Plugins.Dramework4.Runtime.Attributes.Creating;
using IG.HappyCoder.Plugins.Dramework4.Runtime.Attributes.Injecting;
using IG.HappyCoder.Plugins.Dramework4.Runtime.Data;
using IG.HappyCoder.Plugins.Dramework4.Runtime.Interfaces;
using IG.HappyCoder.Plugins.Dramework4.Runtime.Tools;

using Unity.Burst;
using Unity.IL2CPP.CompilerServices;

using UnityEditor;

using UnityEngine;
using UnityEngine.SceneManagement;


namespace IG.HappyCoder.Plugins.Dramework4.Runtime.Core
{
    [BurstCompile]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    internal partial class Dispatcher : MonoBehaviour
    {
        #region ================================ FIELDS

        private const int LABEL_WIDTH = 100;

        private bool _checkScene;
        private bool _isInitializing;
        private bool _lock;
        private string _tag;
        private readonly Dictionary<string, List<Type>> _typesToCreate = new Dictionary<string, List<Type>>();
        private readonly Dictionary<string, CancellationTokenSource> _initializationCTS = new Dictionary<string, CancellationTokenSource>();
        private readonly Dictionary<string, CancellationTokenSource> _startCTS = new Dictionary<string, CancellationTokenSource>();
        private readonly Dictionary<string, SceneContainer> _sceneContainers = new Dictionary<string, SceneContainer>();
        private readonly Dictionary<string, List<ContainerObject<object>>> _objects = new Dictionary<string, List<ContainerObject<object>>>();
        private readonly Queue<string> _scenesToInitialize = new Queue<string>();

        #endregion

        #region ================================ EVENTS AND DELEGATES

        internal static event Action OnEarlyDestroy;
        internal static event Action OnLateDestroy;
        internal static event Action OnStart;

        #endregion

        #region ================================ PROPERTIES AND INDEXERS

        internal static Dispatcher Instance { get; private set; }
        internal Guid SessionID { get; private set; }
        private string TAG
        {
            get
            {
                if (string.IsNullOrEmpty(_tag) == false) return _tag;
                return _tag = $"[{nameof(Dispatcher)}] :";
            }
        }

        #endregion

        #region ================================ METHODS

        internal static bool CheckSession(ref Guid sessionID)
        {
#if UNITY_EDITOR
            if (Instance == null) return false;

            if (Instance.SessionID == sessionID) return true;
            sessionID = Instance.SessionID;
            return false;
#else
            return false;
#endif
        }

        /// <summary>
        /// 1. SubsystemRegistration
        /// 2. AfterAssembliesLoaded
        /// 3. BeforeSplashScreen
        /// 4. BeforeSceneLoad
        /// 5. AfterSceneLoad
        /// </summary>
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void StaticInitialize()
        {
            var go = new GameObject("Dispatcher");
            DontDestroyOnLoad(go);
            go.hideFlags = HideFlags.HideInHierarchy;
            Instance = go.AddComponent<Dispatcher>();
            OnStart = null;
            OnEarlyDestroy = null;
            OnLateDestroy = null;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal void RegisterSceneContainer(SceneContainer sceneContainer)
        {
            _sceneContainers.TryAdd(sceneContainer.gameObject.scene.name, sceneContainer);
        }

        private void Awake()
        {
            SessionID = Guid.NewGuid();
            if (DW4.AppConfig.LogDispatcher)
                DW4.Log($"[Dispatcher]: New session ID:{SessionID}");

            _isInitializing = true;

            Lock();
            Subscribe();
            InitializeTypes();
            InitializePlayerLoopSystems();

            _isInitializing = false;
            Unlock();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void BindObject(string sceneName, object obj)
        {
            var id = obj is IIdentifiable identifiable ? identifiable.ID : string.Empty;
            var containerObject = new ContainerObject<object>(id, sceneName, 0, obj);

            if (_objects.TryGetValue(sceneName, out var objectsList))
                objectsList.Add(containerObject);
            else
                _objects.Add(sceneName, new List<ContainerObject<object>> { containerObject });

            if (DW4.AppConfig.LogDispatcher == false) return;
            DW4.Log(string.IsNullOrEmpty(id)
                ? $"{TAG} Bind object type of \"{obj.GetType()}\""
                : $"{TAG} Bind object type of \"{obj.GetType()}\". ID: \"{id}\"");
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void BindObject(string sceneName, string id, Type type, object obj)
        {
            var bindAttribute = type.GetCustomAttribute<BindAttribute>();
            if (bindAttribute == null) return;

            var containerObject = new ContainerObject<object>(id, sceneName, 0, obj);

            if (_objects.TryGetValue(sceneName, out var objectsList))
                objectsList.Add(containerObject);
            else
                _objects.Add(sceneName, new List<ContainerObject<object>> { containerObject });

            if (DW4.AppConfig.LogDispatcher == false) return;
            DW4.Log(string.IsNullOrEmpty(id)
                ? $"{TAG} Bind object type of \"{type}\""
                : $"{TAG} Bind object type of \"{type}\". ID: \"{id}\"");
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void CreateObjects(string sceneName)
        {
            var createdObjects = new List<object>();

            if (_sceneContainers.TryGetValue(sceneName, out var sceneContainer))
            {
                var containerSceneName = sceneContainer.IsDontDestroyOnLoad ? "DontDestroyOnLoad" : sceneName;
                for (var i = 0; i < sceneContainer.ObjectsToBind.Count; i++)
                {
                    var obj = sceneContainer.ObjectsToBind[i];
                    BindObject(containerSceneName, obj);
                }

                for (var i = 0; i < sceneContainer.DBehaviours.Count; i++)
                {
                    var obj = sceneContainer.DBehaviours[i];
                    createdObjects.Add(obj);

                    var type = obj.GetType();
                    var id = obj is IIdentifiable identifiable ? identifiable.ID : string.Empty;

                    AddToInitializableObjects(containerSceneName, id, type, obj);
                    AddToUpdatableObjects(containerSceneName, id, type, obj);
                    AddToPausableObjects(containerSceneName, id, type, obj);
                    AddToStartableObjects(containerSceneName, id, type, obj);
                }

                _sceneContainers.Remove(sceneName);
            }

            if (_typesToCreate.TryGetValue(sceneName, out var typesToCreate))
            {
                const BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

                for (var i = 0; i < typesToCreate.Count; i++)
                {
                    var type = typesToCreate[i];

                    var constructorsInfo = type.GetConstructors(bindingFlags);
                    var paramValues = new List<object>();
                    for (var j = 0; j < constructorsInfo.Length; j++)
                    {
                        var constructorInfo = constructorsInfo[j];
                        var injectAttribute = constructorInfo.GetCustomAttribute<InjectAttribute>();
                        if (injectAttribute == null) continue;

                        var parameters = constructorInfo.GetParameters();
                        for (var k = 0; k < parameters.Length; k++)
                        {
                            var param = parameters[k];
                            var paramType = param.ParameterType;
                            var idAttribute = param.GetCustomAttribute<IDAttribute>();
                            var paramID = idAttribute == null ? string.Empty : idAttribute.ID;
                            var target = GetObject(sceneName, paramType, paramID);
                            paramValues.Add(target);
                        }
                        break;
                    }

                    var obj = Activator.CreateInstance(type, bindingFlags, null, paramValues.ToArray(), CultureInfo.InvariantCulture);
                    createdObjects.Add(obj);

                    var id = obj is IIdentifiable identifiable ? identifiable.ID : string.Empty;

                    if (DW4.AppConfig.LogDispatcher)
                    {
                        DW4.Log(string.IsNullOrEmpty(id)
                            ? $"{TAG} Create object type of \"{type}\""
                            : $"{TAG} Create object type of \"{type}\". ID: \"{id}\"");
                    }

                    AddToInitializableObjects(sceneName, id, type, obj);
                    AddToUpdatableObjects(sceneName, id, type, obj);
                    AddToPausableObjects(sceneName, id, type, obj);
                    AddToStartableObjects(sceneName, id, type, obj);
                    BindObject(sceneName, id, type, obj);
                }
            }
            else
            {
                if (DW4.AppConfig.LogDispatcher)
                    DW4.LogWarning($"{TAG} Types to create on scene \"{sceneName}\" are not found");
            }

            for (var i = 0; i < createdObjects.Count; i++)
            {
                var obj = createdObjects[i];
                var type = obj.GetType();

                // ReSharper disable once SuspiciousTypeConversion.Global
                if (obj is IPreInitializable preInitializable)
                {
                    preInitializable.OnPreInitialize();
                    if (DW4.AppConfig.LogDispatcher)
                        DW4.Log($"{TAG} Preinitialize object type of \"{type}\"");
                }

                InjectToFields(sceneName, obj, type);
                InjectToProperties(sceneName, obj, type);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void DisposeContainers(string sceneName)
        {
            _objects.Remove(sceneName);
            _sceneContainers.Remove(sceneName);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void DisposeInitializationCTS(string sceneName)
        {
            if (_initializationCTS.TryGetValue(sceneName, out var cts) == false) return;

            if (cts is { IsCancellationRequested: false })
            {
                cts.Cancel();
                cts.Dispose();
            }
            _initializationCTS.Remove(sceneName);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void DisposeStartCTS(string sceneName)
        {
            if (_startCTS.TryGetValue(sceneName, out var cts) == false) return;

            if (cts is { IsCancellationRequested: false })
            {
                cts.Cancel();
                cts.Dispose();
            }
            _startCTS.Remove(sceneName);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private async UniTask InitializeAsync(Scene scene)
        {
            _scenesToInitialize.Enqueue(scene.name);

            if (_isInitializing) return;
            _isInitializing = true;
            Lock();

            while (_scenesToInitialize.Count > 0)
            {
                var sceneName = _scenesToInitialize.Dequeue();

                CreateObjects(sceneName);
                OrderObjects(sceneName);

                await InitializeObjects(sceneName, _initializationCTS[sceneName].Token);
                if (_initializationCTS[sceneName] == null || _initializationCTS[sceneName].IsCancellationRequested) continue;
                DisposeInitializationCTS(sceneName);

                await StartObjects(sceneName, _startCTS[sceneName].Token);
                if (_startCTS[sceneName] == null || _startCTS[sceneName].IsCancellationRequested) continue;
                DisposeStartCTS(sceneName);
            }

            _isInitializing = false;
            Unlock();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void InitializeTypes()
        {
            var result = new List<(string sceneName, int order, Type type)>();

            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                if (IsSystemAssembly(assembly)
                    || IsUnityAssembly(assembly)
                    || IsPluginAssembly(assembly)
                    || IsDW4Assembly(assembly)) continue;

                var types = assembly.GetTypes();
                for (var i = 0; i < types.Length; i++)
                {
                    var type = types[i];
                    var createAttribute = type.GetCustomAttribute<CreateAttribute>();
                    if (createAttribute == null || string.IsNullOrEmpty(createAttribute.SceneName)) continue;
                    result.Add((createAttribute.SceneName, createAttribute.Order, type));
                }
            }

            result = result.OrderBy(t => t.sceneName).ThenBy(t => t.order).ToList();

            for (var i = 0; i < result.Count; i++)
            {
                var item = result[i];

                if (_typesToCreate.TryGetValue(item.sceneName, out var typesToCreate))
                    typesToCreate.Add(item.type);
                else
                    _typesToCreate.Add(item.sceneName, new List<Type> { item.type });
            }
        }

        private void OnApplicationFocus(bool hasFocus)
        {
            if (_isInitializing) return;

            if (hasFocus)
            {
                if (_lock == false) return;
                ResumeObjects();
            }
            else
            {
                if (_lock) return;
                PauseObjects();
            }
        }

        private void OnApplicationPause(bool pauseStatus)
        {
            if (_isInitializing) return;

            if (pauseStatus)
            {
                if (_lock) return;
                PauseObjects();
            }
            else
            {
                if (_lock == false) return;
                ResumeObjects();
            }
        }

        private void OnDestroy()
        {
            Lock();
            Instance = null;
            Unsubscribe();
            OnEarlyDestroy?.Invoke();
            DW4.Dispose();
            OnLateDestroy?.Invoke();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
        {
#if UNITY_EDITOR
            if (scene.buildIndex == 0)
            {
                _checkScene = true;
            }
            else if (_checkScene == false && IsSceneLoaded(0) == false && EditorBuildSettings.scenes.Any(s => s.path == scene.path && s.enabled))
            {
                _checkScene = true;
                // ReSharper disable once Unity.LoadSceneWrongIndex
                SceneManager.LoadScene(0, LoadSceneMode.Single);
                return;
            }
#endif
            try
            {
                var sceneName = scene.name;
                _initializationCTS.Add(sceneName, new CancellationTokenSource());
                _startCTS.Add(sceneName, new CancellationTokenSource());
                InitializeAsync(scene).Forget();
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void OnSceneUnloaded(Scene scene)
        {
            var sceneName = scene.name;

            DisposeInitializationCTS(sceneName);
            DisposeStartCTS(sceneName);
            DisposeContainers(sceneName);
            DisposeInitializableObjects(sceneName);
            DisposeUpdatableObjects(sceneName);
            DisposePausableObjects(sceneName);
            DisposeStartableObjects(sceneName);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void OrderObjects(string sceneName)
        {
            OrderInitializableObjects(sceneName);
            OrderUpdatableObjects();
            OrderPausableObjects(sceneName);
            OrderStartableObjects(sceneName);
        }

        private void Start()
        {
            OnStart?.Invoke();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void Subscribe()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
            SceneManager.sceneUnloaded += OnSceneUnloaded;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void Unsubscribe()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
            SceneManager.sceneUnloaded -= OnSceneUnloaded;
        }

        #endregion
    }
}
