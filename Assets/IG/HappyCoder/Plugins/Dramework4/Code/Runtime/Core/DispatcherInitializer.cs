using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;

using Cysharp.Threading.Tasks;

using IG.HappyCoder.Plugins.Dramework4.Runtime.Attributes.Initialization;
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

        private readonly Dictionary<string, List<ContainerObject<IInitializable>>> _initializables = new Dictionary<string, List<ContainerObject<IInitializable>>>();

        #endregion

        #region ================================ METHODS

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void AddToInitializableObjects(string sceneName, string id, Type type, object obj)
        {
            if (obj is not IInitializable initializable) return;

            var order = type.GetCustomAttribute<InitializeOrderAttribute>()?.Order ?? 0;
            var containerObject = new ContainerObject<IInitializable>(id, sceneName, order, initializable);

            if (_initializables.TryGetValue(sceneName, out var initializables))
                initializables.Add(containerObject);
            else
                _initializables.Add(sceneName, new List<ContainerObject<IInitializable>> { containerObject });

            if (DW4.AppConfig.LogDispatcher == false) return;

            DW4.Log(string.IsNullOrEmpty(id)
                ? $"{TAG} Add Initializable object type of \"{type}\". Order: {order}"
                : $"{TAG} Add Initializable object type of \"{type}\". ID: \"{id}\". Order: {order}");
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void DisposeInitializableObjects(string sceneName)
        {
            if (_initializables.ContainsKey(sceneName) == false) return;

            if (DW4.AppConfig.LogDispatcher)
            {
                for (var i = 0; i < _initializables[sceneName].Count; i++)
                {
                    var obj = _initializables[sceneName][i];
                    DW4.Log(string.IsNullOrEmpty(obj.ID)
                        ? $"{TAG} Remove Initializable object type of \"{obj.Type}\". Order: {obj.Order}"
                        : $"{TAG} Remove Initializable object type of \"{obj.Type}\". ID: \"{obj.ID}\". Order: {obj.Order}");
                }
            }
            _initializables.Remove(sceneName);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private async UniTask InitializeObjects(string sceneName, CancellationToken cancellationToken)
        {
            if (_initializables.TryGetValue(sceneName, out var initializables))
            {
                for (var i = 0; i < initializables.Count; i++)
                {
                    var obj = initializables[i];
                    if (DW4.AppConfig.LogDispatcher)
                    {
                        DW4.Log(string.IsNullOrEmpty(obj.ID)
                            ? $"{TAG} Initialize object type of \"{obj.Type}\". Order: {obj.Order}"
                            : $"{TAG} Initialize object type of \"{obj.Type}\". ID: \"{obj.ID}\". Order: {obj.Order}");
                    }
                    await obj.Object.OnInitialize(cancellationToken);
                    if (cancellationToken.IsCancellationRequested) break;
                }

                DisposeInitializableObjects(sceneName);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void OrderInitializableObjects(string sceneName)
        {
            if (_initializables.TryGetValue(sceneName, out var initializables))
                _initializables[sceneName] = initializables.OrderBy(i => i.Order).ToList();
        }

        #endregion
    }
}