using System.Diagnostics.CodeAnalysis;

using IG.HappyCoder.Plugins.Dramework4.Runtime.Interfaces;

using Sirenix.OdinInspector;

using Unity.Burst;
using Unity.IL2CPP.CompilerServices;

using UnityEngine;


namespace IG.HappyCoder.Plugins.Dramework4.Runtime.Configs
{
    [HideMonoScript]
    [BurstCompile]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    [SuppressMessage("ReSharper", "NotAccessedField.Local")]
    public class DW4ConfigAsset<T> : ScriptableObject, IIdentifiable where T : IIdentifiable
    {
        #region ================================ FIELDS

        private const int LABEL_WIDTH = 100;

        [FoldoutGroup("Asset")] [BoxGroup("Asset/ID", false)]
        [LabelWidth(LABEL_WIDTH)] [LabelText("ID:")]
        [SerializeField] [ReadOnly]
        private string _id;

        [SerializeField] [HideLabel] [ReadOnly]
        private T _config;

        #endregion

        #region ================================ PROPERTIES AND INDEXERS

        public T Config => _config;
        public string ID
        {
            get => _id;
            set => _id = value;
        }

        #endregion

        #region ================================ METHODS

        private void Initialize(T config)
        {
            _config = config;
        }

        #endregion
    }
}