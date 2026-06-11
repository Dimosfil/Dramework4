using IG.HappyCoder.Plugins.Dramework4.Runtime.Attributes.Getting;
using IG.HappyCoder.Plugins.Dramework4.Runtime.Attributes.Injecting;
using IG.HappyCoder.Plugins.Dramework4.Runtime.Behaviours;
using IG.HappyCoder.Plugins.Dramework4.Runtime.Interfaces;

using Sirenix.OdinInspector;

using Unity.Burst;
using Unity.IL2CPP.CompilerServices;

using UnityEngine;


namespace IG.HappyCoder.Plugins.Dramework4.Runtime.Binders
{
    [BurstCompile] [HideMonoScript] [Bind]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public class CameraBinder : DBehaviour, IIdentifiable
    {
        #region ================================ FIELDS

        [BoxGroup("ID", false)]
        [LabelText("ID:")]
        [SerializeField]
        private string _id;

        [BoxGroup("Camera", false)]
        [LabelText("Camera:")]
        [SerializeField] [GetComponent] [ReadOnly]
        private Camera _camera;

        #endregion

        #region ================================ PROPERTIES AND INDEXERS

        public Camera Camera => _camera;
        string IIdentifiable.ID => string.IsNullOrEmpty(_id) ? name : _id;

        #endregion
    }
}