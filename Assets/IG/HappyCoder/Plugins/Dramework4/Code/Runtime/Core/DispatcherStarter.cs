using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;

using Cysharp.Threading.Tasks;

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

        private readonly Dictionary<string, List<ContainerObject<IStartable>>> _startables = new Dictionary<string, List<ContainerObject<IStartable>>>();

        #endregion

        #region ================================ METHODS

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void AddToStartableObjects(string sceneName, string id, Type type, object obj)
        {
            if (obj is not IStartable startable) return;

            var order = type.GetCustomAttribute<StartOrderAttribute>()?.Order ?? 0;
            var containerObject = new ContainerObject<IStartable>(id, sceneName, order, startable);

            if (_startables.TryGetValue(sceneName, out var startables))
                startables.Add(containerObject);
            else
                _startables.Add(sceneName, new List<ContainerObject<IStartable>> { containerObject });

            if (DW4.AppConfig.LogDispatcher == false) return;

            DW4.Log(string.IsNullOrEmpty(id)
                ? $"{TAG} Add Startable object type of \"{type}\". Order: {order}"
                : $"{TAG} Add Startable object type of \"{type}\". ID: \"{id}\". Order: {order}");
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void DisposeStartableObjects(string sceneName)
        {
            if (_startables.ContainsKey(sceneName) == false) return;

            if (DW4.AppConfig.LogDispatcher)
            {
                for (var i = 0; i < _startables[sceneName].Count; i++)
                {
                    var obj = _startables[sceneName][i];
                    DW4.Log(string.IsNullOrEmpty(obj.ID)
                        ? $"{TAG} Remove Startable object type of \"{obj.Type}\". Order: {obj.Order}"
                        : $"{TAG} Remove Startable object type of \"{obj.Type}\". ID: \"{obj.ID}\". Order: {obj.Order}");
                }
            }
            _startables.Remove(sceneName);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void OrderStartableObjects(string sceneName)
        {
            if (_startables.TryGetValue(sceneName, out var startables))
                _startables[sceneName] = startables.OrderBy(i => i.Order).ToList();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private async UniTask StartObjects(string sceneName, CancellationToken cancellationToken)
        {
            if (_startables.TryGetValue(sceneName, out var startables))
            {
                for (var i = 0; i < startables.Count; i++)
                {
                    var obj = startables[i];
                    if (DW4.AppConfig.LogDispatcher)
                    {
                        DW4.Log(string.IsNullOrEmpty(obj.ID)
                            ? $"{TAG} Start object type of \"{obj.Type}\". Order: {obj.Order}"
                            : $"{TAG} Start object type of \"{obj.Type}\". ID: \"{obj.ID}\". Order: {obj.Order}");
                    }
                    await obj.Object.OnStart(cancellationToken);
                    if (cancellationToken.IsCancellationRequested) break;
                }

                DisposeStartableObjects(sceneName);
            }
        }

        #endregion
    }
}