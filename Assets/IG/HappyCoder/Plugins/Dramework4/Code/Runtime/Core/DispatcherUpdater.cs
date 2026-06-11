using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

using IG.HappyCoder.Plugins.Dramework4.Runtime.Attributes.Updating;
using IG.HappyCoder.Plugins.Dramework4.Runtime.Data;
using IG.HappyCoder.Plugins.Dramework4.Runtime.Interfaces;
using IG.HappyCoder.Plugins.Dramework4.Runtime.Tools;

using Sirenix.Utilities;

using Unity.IL2CPP.CompilerServices;

using UnityEngine.LowLevel;
using UnityEngine.PlayerLoop;


namespace IG.HappyCoder.Plugins.Dramework4.Runtime.Core
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    internal partial class Dispatcher
    {
        #region ================================ FIELDS

        private List<ContainerObject<IFixedUpdatable>> _fixedUpdatables = new List<ContainerObject<IFixedUpdatable>>();
        private List<ContainerObject<IEarlyUpdatable>> _earlyUpdatables = new List<ContainerObject<IEarlyUpdatable>>();
        private List<ContainerObject<IPreUpdatable>> _preUpdatables = new List<ContainerObject<IPreUpdatable>>();
        private List<ContainerObject<IUpdatable>> _updatables = new List<ContainerObject<IUpdatable>>();
        private List<ContainerObject<IPreLateUpdatable>> _preLateUpdatables = new List<ContainerObject<IPreLateUpdatable>>();
        private List<ContainerObject<IPostLateUpdatable>> _postLateUpdatables = new List<ContainerObject<IPostLateUpdatable>>();
        private bool _lockUpdate;

        #endregion

        #region ================================ METHODS

        // ReSharper disable once CyclomaticComplexity
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void AddToUpdatableObjects(string sceneName, string id, Type type, object obj)
        {
            if (obj is IEarlyUpdatable earlyUpdatable)
            {
                var order = type.GetCustomAttribute<EarlyUpdateOrderAttribute>()?.Order ?? 0;

                _earlyUpdatables.Add(new ContainerObject<IEarlyUpdatable>(id, sceneName, order, earlyUpdatable));

                if (DW4.AppConfig.LogDispatcher)
                {
                    DW4.Log(string.IsNullOrEmpty(id)
                        ? $"{TAG} Add EarlyUpdatable object type of \"{type}\". Order: {order}"
                        : $"{TAG} Add EarlyUpdatable object type of \"{type}\". ID: \"{id}\". Order: {order}");
                }
            }

            if (obj is IFixedUpdatable fixedUpdatable)
            {
                var order = type.GetCustomAttribute<FixedUpdateOrderAttribute>()?.Order ?? 0;

                _fixedUpdatables.Add(new ContainerObject<IFixedUpdatable>(id, sceneName, order, fixedUpdatable));

                if (DW4.AppConfig.LogDispatcher)
                {
                    DW4.Log(string.IsNullOrEmpty(id)
                        ? $"{TAG} Add FixedUpdatable object type of \"{type}\". Order: {order}"
                        : $"{TAG} Add FixedUpdatable object type of \"{type}\". ID: \"{id}\". Order: {order}");
                }
            }

            if (obj is IPreUpdatable preUpdatable)
            {
                var order = type.GetCustomAttribute<PreUpdateOrderAttribute>()?.Order ?? 0;

                _preUpdatables.Add(new ContainerObject<IPreUpdatable>(id, sceneName, order, preUpdatable));

                if (DW4.AppConfig.LogDispatcher)
                {
                    DW4.Log(string.IsNullOrEmpty(id)
                        ? $"{TAG} Add PreUpdatable object type of \"{type}\". Order: {order}"
                        : $"{TAG} Add PreUpdatable object type of \"{type}\". ID: \"{id}\". Order: {order}");
                }
            }

            if (obj is IUpdatable updatable)
            {
                var order = type.GetCustomAttribute<UpdateOrderAttribute>()?.Order ?? 0;

                _updatables.Add(new ContainerObject<IUpdatable>(id, sceneName, order, updatable));

                if (DW4.AppConfig.LogDispatcher)
                {
                    DW4.Log(string.IsNullOrEmpty(id)
                        ? $"{TAG} Add Updatable object type of \"{type}\". Order: {order}"
                        : $"{TAG} Add Updatable object type of \"{type}\". ID: \"{id}\". Order: {order}");
                }
            }

            if (obj is IPreLateUpdatable preLateUpdatable)
            {
                var order = type.GetCustomAttribute<PreLateUpdateOrderAttribute>()?.Order ?? 0;

                _preLateUpdatables.Add(new ContainerObject<IPreLateUpdatable>(id, sceneName, order, preLateUpdatable));

                if (DW4.AppConfig.LogDispatcher)
                {
                    DW4.Log(string.IsNullOrEmpty(id)
                        ? $"{TAG} Add PreLateUpdatable object type of \"{type}\". Order: {order}"
                        : $"{TAG} Add PreLateUpdatable object type of \"{type}\". ID: \"{id}\". Order: {order}");
                }
            }

            if (obj is IPostLateUpdatable postLateUpdatable)
            {
                var order = type.GetCustomAttribute<PostLateUpdateOrderAttribute>()?.Order ?? 0;

                _postLateUpdatables.Add(new ContainerObject<IPostLateUpdatable>(id, sceneName, order, postLateUpdatable));

                if (DW4.AppConfig.LogDispatcher)
                {
                    DW4.Log(string.IsNullOrEmpty(id)
                        ? $"{TAG} Add PostLateUpdatable object type of \"{type}\". Order: {order}"
                        : $"{TAG} Add PostLateUpdatable object type of \"{type}\". ID: \"{id}\". Order: {order}");
                }
            }
        }

        // ReSharper disable once CyclomaticComplexity
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void DisposeUpdatableObjects(string sceneName)
        {
            for (var i = _earlyUpdatables.Count - 1; i >= 0; i--)
            {
                var obj = _earlyUpdatables[i];
                if (obj.SceneName != sceneName) continue;

                if (DW4.AppConfig.LogDispatcher)
                {
                    DW4.Log(string.IsNullOrEmpty(obj.ID)
                        ? $"{TAG} Remove EarlyUpdatable object type of \"{obj.Type}\". Order: {obj.Order}"
                        : $"{TAG} Remove EarlyUpdatable object type of \"{obj.Type}\". ID: \"{obj.ID}\". Order: {obj.Order}");
                }
                _earlyUpdatables.RemoveAt(i);
            }

            for (var i = _fixedUpdatables.Count - 1; i >= 0; i--)
            {
                var obj = _fixedUpdatables[i];
                if (obj.SceneName != sceneName) continue;

                if (DW4.AppConfig.LogDispatcher)
                {
                    DW4.Log(string.IsNullOrEmpty(obj.ID)
                        ? $"{TAG} Remove FixedUpdatable object type of \"{obj.Type}\". Order: {obj.Order}"
                        : $"{TAG} Remove FixedUpdatable object type of \"{obj.Type}\". ID: \"{obj.ID}\". Order: {obj.Order}");
                }
                _fixedUpdatables.RemoveAt(i);
            }

            for (var i = _preUpdatables.Count - 1; i >= 0; i--)
            {
                var obj = _preUpdatables[i];
                if (obj.SceneName != sceneName) continue;

                if (DW4.AppConfig.LogDispatcher)
                {
                    DW4.Log(string.IsNullOrEmpty(obj.ID)
                        ? $"{TAG} Remove PreUpdatable object type of \"{obj.Type}\". Order: {obj.Order}"
                        : $"{TAG} Remove PreUpdatable object type of \"{obj.Type}\". ID: \"{obj.ID}\". Order: {obj.Order}");
                }
                _preUpdatables.RemoveAt(i);
            }

            for (var i = _updatables.Count - 1; i >= 0; i--)
            {
                var obj = _updatables[i];
                if (obj.SceneName != sceneName) continue;

                if (DW4.AppConfig.LogDispatcher)
                {
                    DW4.Log(string.IsNullOrEmpty(obj.ID)
                        ? $"{TAG} Remove Updatable object type of \"{obj.Type}\". Order: {obj.Order}"
                        : $"{TAG} Remove Updatable object type of \"{obj.Type}\". ID: \"{obj.ID}\". Order: {obj.Order}");
                }
                _updatables.RemoveAt(i);
            }

            for (var i = _preLateUpdatables.Count - 1; i >= 0; i--)
            {
                var obj = _preLateUpdatables[i];
                if (obj.SceneName != sceneName) continue;

                if (DW4.AppConfig.LogDispatcher)
                {
                    DW4.Log(string.IsNullOrEmpty(obj.ID)
                        ? $"{TAG} Remove PreLateUpdatable object type of \"{obj.Type}\". Order: {obj.Order}"
                        : $"{TAG} Remove PreLateUpdatable object type of \"{obj.Type}\". ID: \"{obj.ID}\". Order: {obj.Order}");
                }
                _preLateUpdatables.RemoveAt(i);
            }

            for (var i = _postLateUpdatables.Count - 1; i >= 0; i--)
            {
                var obj = _postLateUpdatables[i];
                if (obj.SceneName != sceneName) continue;

                if (DW4.AppConfig.LogDispatcher)
                {
                    DW4.Log(string.IsNullOrEmpty(obj.ID)
                        ? $"{TAG} Remove PostLateUpdatable object type of \"{obj.Type}\". Order: {obj.Order}"
                        : $"{TAG} Remove PostLateUpdatable object type of \"{obj.Type}\". ID: \"{obj.ID}\". Order: {obj.Order}");
                }
                _postLateUpdatables.RemoveAt(i);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void EarlyUpdateObjects()
        {
            if (_lock || _lockUpdate) return;

            for (var i = 0; i < _earlyUpdatables.Count; i++)
                _earlyUpdatables[i].Object.OnEarlyUpdate();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void FixedUpdateObjects()
        {
            if (_lock || _lockUpdate) return;

            for (var i = 0; i < _fixedUpdatables.Count; i++)
                _fixedUpdatables[i].Object.OnFixedUpdate();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void InitializePlayerLoopSystems()
        {
#if UNITY_EDITOR

            if (DW4.IsDomainReloadDisabled)
            {
                DW4.UnityPlayerLoopTools.RemoveSystem<EarlyUpdate>(typeof(Dispatcher));
                if (DW4.AppConfig.LogDispatcher)
                    DW4.Log($"{TAG} Remove EarlyUpdate player loop system");
                DW4.UnityPlayerLoopTools.RemoveSystem<FixedUpdate>(typeof(Dispatcher));
                if (DW4.AppConfig.LogDispatcher)
                    DW4.Log($"{TAG} Remove FixedUpdate player loop system");
                DW4.UnityPlayerLoopTools.RemoveSystem<PreUpdate>(typeof(Dispatcher));
                if (DW4.AppConfig.LogDispatcher)
                    DW4.Log($"{TAG} Remove PreUpdate player loop system");
                DW4.UnityPlayerLoopTools.RemoveSystem<Update>(typeof(Dispatcher));
                if (DW4.AppConfig.LogDispatcher)
                    DW4.Log($"{TAG} Remove Update player loop system");
                DW4.UnityPlayerLoopTools.RemoveSystem<PreLateUpdate>(typeof(Dispatcher));
                if (DW4.AppConfig.LogDispatcher)
                    DW4.Log($"{TAG} Remove PreLateUpdate player loop system");
                DW4.UnityPlayerLoopTools.RemoveSystem<PostLateUpdate>(typeof(Dispatcher));
                if (DW4.AppConfig.LogDispatcher)
                    DW4.Log($"{TAG} Remove PostLateUpdate player loop system");
                DW4.UnityPlayerLoopTools.RemoveSystem<TimeUpdate>(typeof(Dispatcher));
                if (DW4.AppConfig.LogDispatcher)
                    DW4.Log($"{TAG} Remove TimeUpdate player loop system");
            }

#endif
            var earlyUpdateSystem = new PlayerLoopSystem
            {
                subSystemList = null,
                updateDelegate = EarlyUpdateObjects,
                type = typeof(Dispatcher)
            };
            DW4.UnityPlayerLoopTools.AddSystem<EarlyUpdate>(earlyUpdateSystem);
            if (DW4.AppConfig.LogDispatcher)
                DW4.Log($"{TAG} Add EarlyUpdate player loop system");

            var fixedUpdateSystem = new PlayerLoopSystem
            {
                subSystemList = null,
                updateDelegate = FixedUpdateObjects,
                type = typeof(Dispatcher)
            };
            DW4.UnityPlayerLoopTools.InsertSystem<FixedUpdate>(6, fixedUpdateSystem);
            if (DW4.AppConfig.LogDispatcher)
                DW4.Log($"{TAG} Add FixedUpdate player loop system");

            var preUpdateSystem = new PlayerLoopSystem
            {
                subSystemList = null,
                updateDelegate = PreUpdateObjects,
                type = typeof(Dispatcher)
            };
            DW4.UnityPlayerLoopTools.AddSystem<PreUpdate>(preUpdateSystem);
            if (DW4.AppConfig.LogDispatcher)
                DW4.Log($"{TAG} Add PreUpdate player loop system");

            var updateSystem = new PlayerLoopSystem
            {
                subSystemList = null,
                updateDelegate = UpdateObjects,
                type = typeof(Dispatcher)
            };
            DW4.UnityPlayerLoopTools.InsertSystem<Update>(0, updateSystem);
            if (DW4.AppConfig.LogDispatcher)
                DW4.Log($"{TAG} Add Update player loop system");

            var preLateUpdateSystem = new PlayerLoopSystem
            {
                subSystemList = null,
                updateDelegate = PreLateUpdateObjects,
                type = typeof(Dispatcher)
            };
            DW4.UnityPlayerLoopTools.InsertSystem<PreLateUpdate>(14, preLateUpdateSystem);
            if (DW4.AppConfig.LogDispatcher)
                DW4.Log($"{TAG} Add PreLateUpdate player loop system");

            var postLateUpdateSystem = new PlayerLoopSystem
            {
                subSystemList = null,
                updateDelegate = PostLateUpdateObjects,
                type = typeof(Dispatcher)
            };
            DW4.UnityPlayerLoopTools.AddSystem<PostLateUpdate>(postLateUpdateSystem);
            if (DW4.AppConfig.LogDispatcher)
                DW4.Log($"{TAG} Add PostLateUpdate player loop system");

            var timeUpdateSystem = new PlayerLoopSystem
            {
                subSystemList = null,
                updateDelegate = TimeUpdate,
                type = typeof(Dispatcher)
            };
            DW4.UnityPlayerLoopTools.AddSystem<TimeUpdate>(timeUpdateSystem);
            if (DW4.AppConfig.LogDispatcher)
                DW4.Log($"{TAG} Add TimeUpdate player loop system");
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void OrderUpdatableObjects()
        {
            _fixedUpdatables = _fixedUpdatables.OrderBy(o => o.Order).ToList();
            _earlyUpdatables = _earlyUpdatables.OrderBy(o => o.Order).ToList();
            _preUpdatables = _preUpdatables.OrderBy(o => o.Order).ToList();
            _updatables = _updatables.OrderBy(o => o.Order).ToList();
            _preLateUpdatables = _preLateUpdatables.OrderBy(o => o.Order).ToList();
            _postLateUpdatables = _postLateUpdatables.OrderBy(o => o.Order).ToList();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void PostLateUpdateObjects()
        {
            if (_lock || _lockUpdate) return;

            for (var i = 0; i < _postLateUpdatables.Count; i++)
                _postLateUpdatables[i].Object.OnPostLateUpdate();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void PreLateUpdateObjects()
        {
            if (_lock || _lockUpdate) return;

            for (var i = 0; i < _preLateUpdatables.Count; i++)
                _preLateUpdatables[i].Object.OnPreLateUpdate();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void PreUpdateObjects()
        {
            if (_lock || _lockUpdate) return;

            for (var i = 0; i < _preUpdatables.Count; i++)
                _preUpdatables[i].Object.OnPreUpdate();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void TimeUpdate()
        {
            // if (Application.isPlaying == false || _lock) return;
            // DTimer.Update();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void UpdateObjects()
        {
            if (_lock || _lockUpdate) return;

            for (var i = 0; i < _updatables.Count; i++)
                _updatables[i].Object.OnUpdate();
        }

        #endregion
    }
}