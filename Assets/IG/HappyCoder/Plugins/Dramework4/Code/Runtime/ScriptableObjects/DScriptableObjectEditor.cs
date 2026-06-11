#if UNITY_EDITOR

using Sirenix.OdinInspector;

using Unity.IL2CPP.CompilerServices;

using UnityEditor;

using UnityEngine;


namespace IG.HappyCoder.Plugins.Dramework4.Runtime.ScriptableObjects
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public partial class DScriptableObject
    {
        #region ================================ FIELDS

        [SerializeField] [HideInInspector]
        private Texture _logo;

        #endregion

        #region ================================ PROPERTIES AND INDEXERS

        protected bool DisableSaveButton { get; set; }

        protected bool HideSaveButton { get; set; }

        #endregion

        #region ================================ METHODS

        [BoxGroup("Buttons", false, order: -99999)] [VerticalGroup("Buttons/Vertical")] [HorizontalGroup("Buttons/Vertical/Horizontal1")]
        [Button("Save", ButtonSizes.Medium)] [PropertyOrder(1)] [HideIf(nameof(HideSaveButton))] [DisableIf(nameof(DisableSaveButton))]
        public virtual void Save()
        {
            if (EditorApplication.isPlayingOrWillChangePlaymode) return;

            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            EditorApplication.ExecuteMenuItem("File/Save Project");
        }

        [OnInspectorGUI]
        [PropertyOrder(-100000)]
        private void DrawHeader()
        {
            if (_logo == null)
                _logo = Resources.Load<Texture>(EditorGUIUtility.isProSkin ? "D Framework 4 Logo Dark" : "D Framework 4 Logo Light");

            GUILayout.Label(_logo);
        }

        #endregion
    }
}

#endif