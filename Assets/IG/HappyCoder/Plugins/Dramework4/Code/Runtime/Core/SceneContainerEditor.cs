#if UNITY_EDITOR

using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;

using IG.HappyCoder.Plugins.Dramework4.Runtime.Attributes.Injecting;
using IG.HappyCoder.Plugins.Dramework4.Runtime.Behaviours;

using Sirenix.OdinInspector;

using Unity.IL2CPP.CompilerServices;

using UnityEditor;

using UnityEngine;


namespace IG.HappyCoder.Plugins.Dramework4.Runtime.Core
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    internal partial class SceneContainer
    {
        #region ================================ METHODS

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void FindAllDBehaviours()
        {
            var result = new List<DBehaviour>();
            foreach (var rootGameObject in gameObject.scene.GetRootGameObjects())
                result.AddRange(rootGameObject.GetComponentsInChildren<DBehaviour>(true));

            _dBehaviours = result.ToArray();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void FindObjectsToBind()
        {
            var result = new List<Component>();
            foreach (var dBehaviour in _dBehaviours)
            {
                var bindAttribute = dBehaviour.GetType().GetCustomAttribute<BindAttribute>();
                if (bindAttribute != null)
                    result.Add(dBehaviour);
            }
            _objectsToBind = result.ToArray();
        }

        private void OnValidate()
        {
            if (EditorApplication.isPlayingOrWillChangePlaymode || gameObject.scene.isLoaded == false) return;
            Refresh();
        }

        [BoxGroup("Buttons", false, order: -10000)] [HorizontalGroup("Buttons/Horizontal")]
        [Button("Refresh", ButtonSizes.Medium)]
        internal void Refresh()
        {
            FindAllDBehaviours();
            FindObjectsToBind();
        }

        #endregion
    }
}

#endif