#if UNITY_EDITOR

using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Reflection;

using IG.HappyCoder.Plugins.Dramework4.Runtime.Configs;
using IG.HappyCoder.Plugins.Dramework4.Runtime.Constants;
using IG.HappyCoder.Plugins.Dramework4.Runtime.ScriptableObjects;
using IG.HappyCoder.Plugins.Dramework4.Runtime.Tools;

using Sirenix.OdinInspector;

using Unity.Burst;
using Unity.IL2CPP.CompilerServices;

using UnityEditor;

using UnityEngine;


namespace IG.HappyCoder.Plugins.Dramework4.Runtime.Configurators
{
    [HideMonoScript]
    [BurstCompile]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    [CreateAssetMenu(menuName = "DW4/Configurators/Storage Data", fileName = "Storage Data Configurator")]
    [SuppressMessage("ReSharper", "ClassWithVirtualMembersNeverInherited.Global")]
    internal class StorageDataConfigurator : DScriptableObject
    {
        #region ================================ FIELDS

        private const int LABEL_WIDTH = 110;
        private const string ASSET_EXTENSION = "asset";

        [SerializeField] [HideLabel] [DisableIf("_lock")]
        private ConfigExportSettings _exportSettings;

        [SerializeField] [HideLabel] [DisableIf("_lock")]
        private StorageDataConfig _config;

        [SerializeField] [HideInInspector]
#pragma warning disable CS0414 // Field is assigned but its value is never used
        private bool _lock;
#pragma warning restore CS0414 // Field is assigned but its value is never used

        [SerializeField] [HideInInspector]
        private string _path;

        #endregion

        #region ================================ METHODS

        [BoxGroup("Buttons", false, order: -99999)] [VerticalGroup("Buttons/Vertical")] [HorizontalGroup("Buttons/Vertical/Horizontal1")]
        [Button("Export", ButtonSizes.Medium)] [PropertyOrder(3)]
        public virtual void Export()
        {
            if (EditorApplication.isPlayingOrWillChangePlaymode) return;

            var storageLogEnable = DW4.AppConfig.LogStorage;
            var filedInfo = DW4.AppConfig.GetType().GetField("_storageLogEnable", BindingFlags.Instance | BindingFlags.NonPublic);
            filedInfo?.SetValue(DW4.AppConfig, false);

            var id = string.IsNullOrEmpty(_config.ID) ? "storage_data" : _config.ID;
            var path = EditorUtility.SaveFilePanelInProject("Export",
                string.IsNullOrEmpty(_path) ? id : Path.GetFileNameWithoutExtension(_path),
                _exportSettings.Extension,
                "",
                string.IsNullOrEmpty(_path) ? "Assets" : Path.GetDirectoryName(_path));

            if (string.IsNullOrEmpty(path)) return;

            _path = path;
            var exportConfig = new StorageDataConfig(
                id,
                _path,
                StorageType.File,
                _exportSettings.SerializationType,
                _exportSettings.EncryptionType,
                _exportSettings.XorKey,
                _exportSettings.AesKey,
                _exportSettings.AesIV,
                _exportSettings.MD5Key
            );

            exportConfig.Save(_config);

            var container = CreateInstance<StorageDataConfigAsset>();
            var methodInfo = container.GetType().GetMethod("Initialize", BindingFlags.Instance | BindingFlags.NonPublic);
            methodInfo?.Invoke(container, new object[] { _config });
            AssetDatabase.CreateAsset(container, _path.Replace(_exportSettings.Extension, ASSET_EXTENSION));

            Save();

            filedInfo?.SetValue(DW4.AppConfig, storageLogEnable);
        }

        [BoxGroup("Buttons", false, order: -99999)] [VerticalGroup("Buttons/Vertical")] [HorizontalGroup("Buttons/Vertical/Horizontal1")]
        [Button("Import", ButtonSizes.Medium)] [PropertyOrder(2)]
        public virtual void Import()
        {
            if (EditorApplication.isPlayingOrWillChangePlaymode) return;

            var storageLogEnable = DW4.AppConfig.LogStorage;
            var filedInfo = DW4.AppConfig.GetType().GetField("_storageLogEnable", BindingFlags.Instance | BindingFlags.NonPublic);
            filedInfo?.SetValue(DW4.AppConfig, false);

            var path = EditorUtility.OpenFilePanel("Import", string.IsNullOrEmpty(_path) ? "Assets" : Path.GetDirectoryName(_path), _exportSettings.Extension);
            if (string.IsNullOrEmpty(path)) return;

            var importConfig = new StorageDataConfig(
                string.Empty,
                path,
                StorageType.File,
                _exportSettings.SerializationType,
                _exportSettings.EncryptionType,
                _exportSettings.XorKey,
                _exportSettings.AesKey,
                _exportSettings.AesIV,
                _exportSettings.MD5Key
            );

            var response = importConfig.Load<StorageDataConfig>();
            if (response.Success)
                _config = response.Data;

            Save();

            filedInfo?.SetValue(DW4.AppConfig, storageLogEnable);
        }

        [BoxGroup("Buttons", false, order: -99999)] [VerticalGroup("Buttons/Vertical")] [HorizontalGroup("Buttons/Vertical/Horizontal1")]
        [Button("Lock", ButtonSizes.Medium)] [HideIf("_lock")] [PropertyOrder(4)]
        public virtual void Lock()
        {
            _lock = true;
        }

        [BoxGroup("Buttons", false, order: -99999)] [VerticalGroup("Buttons/Vertical")] [HorizontalGroup("Buttons/Vertical/Horizontal1")]
        [Button("Unlock", ButtonSizes.Medium)] [ShowIf("_lock")] [PropertyOrder(4)]
        public virtual void Unlock()
        {
            _lock = false;
        }

        #endregion
    }
}

#endif