using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

using IG.HappyCoder.Plugins.Dramework4.Runtime.Attributes.Updating;
using IG.HappyCoder.Plugins.Dramework4.Runtime.Data;
using IG.HappyCoder.Plugins.Dramework4.Runtime.Interfaces;
using IG.HappyCoder.Plugins.Dramework4.Runtime.Tools;

using Unity.IL2CPP.CompilerServices;


namespace IG.HappyCoder.Plugins.Dramework4.Runtime.Core
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    internal partial class Dispatcher
    {
        #region ================================ FIELDS

        private readonly Dictionary<string, List<ContainerObject<IPausable>>> _pausables = new Dictionary<string, List<ContainerObject<IPausable>>>();

        #endregion

        #region ================================ METHODS

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void AddToPausableObjects(string sceneName, string id, Type type, object obj)
        {
            if (obj is not IPausable pausable) return;

            var order = type.GetCustomAttribute<PauseOrderAttribute>()?.Order ?? 0;
            var containerObject = new ContainerObject<IPausable>(id, sceneName, order, pausable);

            if (_pausables.TryGetValue(sceneName, out var pausables))
                pausables.Add(containerObject);
            else
                _pausables.Add(sceneName, new List<ContainerObject<IPausable>> { containerObject });

            if (DW4.AppConfig.LogDispatcher == false) return;

            DW4.Log(string.IsNullOrEmpty(id)
                ? $"{TAG} Add Pausable object type of \"{type}\". Order: {order}"
                : $"{TAG} Add Pausable object type of \"{type}\". ID: \"{id}\". Order: {order}");
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void DisposePausableObjects(string sceneName)
        {
            if (_pausables.ContainsKey(sceneName) == false) return;

            if (DW4.AppConfig.LogDispatcher)
            {
                for (var i = 0; i < _pausables[sceneName].Count; i++)
                {
                    var obj = _pausables[sceneName][i];
                    DW4.Log(string.IsNullOrEmpty(obj.ID)
                        ? $"{TAG} Remove Pausable object type of \"{obj.Type}\". Order: {obj.Order}"
                        : $"{TAG} Remove Pausable object type of \"{obj.Type}\". ID: \"{obj.ID}\". Order: {obj.Order}");
                }
            }
            _pausables.Remove(sceneName);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void OrderPausableObjects(string sceneName)
        {
            if (_pausables.TryGetValue(sceneName, out var pausables))
                _pausables[sceneName] = pausables.OrderBy(i => i.Order).ToList();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void PauseObjects()
        {
            Lock();

            foreach (var pausables in _pausables.Values)
            {
                for (var i = 0; i < pausables.Count; i++)
                {
                    var obj = pausables[i];
                    obj.Object.OnPause();

                    if (DW4.AppConfig.LogDispatcher == false) continue;

                    DW4.Log(string.IsNullOrEmpty(obj.ID)
                        ? $"{TAG} Pause object type of \"{obj.Type}\". Order: {obj.Order}"
                        : $"{TAG} Pause object type of \"{obj.Type}\". ID: \"{obj.ID}\". Order: {obj.Order}");
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void ResumeObjects()
        {
            Unlock();

            foreach (var pausables in _pausables.Values)
            {
                for (var i = 0; i < pausables.Count; i++)
                {
                    var obj = pausables[i];
                    obj.Object.OnResume();

                    if (DW4.AppConfig.LogDispatcher == false) continue;

                    DW4.Log(string.IsNullOrEmpty(obj.ID)
                        ? $"{TAG} Resume object type of \"{obj.Type}\". Order: {obj.Order}"
                        : $"{TAG} Resume object type of \"{obj.Type}\". ID: \"{obj.ID}\". Order: {obj.Order}");
                }
            }
        }

        #endregion
    }
}
