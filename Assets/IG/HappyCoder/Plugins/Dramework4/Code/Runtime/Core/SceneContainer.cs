using System.Collections.Generic;

using IG.HappyCoder.Plugins.Dramework4.Runtime.Behaviours;

using Sirenix.OdinInspector;

using Unity.Burst;
using Unity.IL2CPP.CompilerServices;

using UnityEngine;


namespace IG.HappyCoder.Plugins.Dramework4.Runtime.Core
{
    [BurstCompile] [HideMonoScript]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    internal partial class SceneContainer : MonoBehaviour
    {
        #region ================================ FIELDS

        [BoxGroup("Don't Destroy on Load", false)]
        [LabelText("Don't Destroy on Load:")]
        [SerializeField]
        private bool _dontDestroyOnLoad;

        [BoxGroup("DBehaviours", false)]
        [LabelText("DBehaviours:")]
        [SerializeField] [ListDrawerSettings(HideAddButton = false, HideRemoveButton = false)] [ReadOnly]
        private DBehaviour[] _dBehaviours;

        [BoxGroup("Objects To Bind", false)]
        [LabelText("Objects To Bind:")]
        [SerializeField] [ListDrawerSettings(HideAddButton = false, HideRemoveButton = false)] [ReadOnly]
        private Component[] _objectsToBind;

        #endregion

        #region ================================ PROPERTIES AND INDEXERS

        internal IReadOnlyList<DBehaviour> DBehaviours => _dBehaviours;
        internal bool IsDontDestroyOnLoad => _dontDestroyOnLoad;
        internal IReadOnlyList<Component> ObjectsToBind => _objectsToBind;

        #endregion

        #region ================================ METHODS

        private void Awake()
        {
            Dispatcher.Instance.RegisterSceneContainer(this);

            if (_dontDestroyOnLoad == false) return;
            foreach (var rootGameObject in gameObject.scene.GetRootGameObjects())
                DontDestroyOnLoad(rootGameObject);
        }

        #endregion
    }
}