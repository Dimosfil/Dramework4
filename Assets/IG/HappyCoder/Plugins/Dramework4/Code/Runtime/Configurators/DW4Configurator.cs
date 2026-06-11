#if UNITY_EDITOR

using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Reflection;

using IG.HappyCoder.Plugins.Dramework4.Runtime.Configs;
using IG.HappyCoder.Plugins.Dramework4.Runtime.Constants;
using IG.HappyCoder.Plugins.Dramework4.Runtime.Interfaces;
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
    [SuppressMessage("ReSharper", "ClassWithVirtualMembersNeverInherited.Global")]
    public class DW4Configurator<TConfig, TConfigAsset> : DScriptableObject where TConfig : IIdentifiable where TConfigAsset : DW4ConfigAsset<TConfig>
    {
        #region ================================ FIELDS

        private const int LABEL_WIDTH = 110;
        private const string ASSET_EXTENSION = "asset";

        [SerializeField] [HideLabel] [DisableIf("_lock")]
        private ConfigExportSettings _exportSettings;

        [FoldoutGroup("Asset")] [BoxGroup("Asset/ID", false)]
        [LabelWidth(LABEL_WIDTH)] [LabelText("ID:")]
        [SerializeField] [ReadOnly]
        private string _assetID;

        [SerializeField] [HideLabel] [DisableIf("_lock")]
        private TConfig _config;

        [SerializeField] [HideInInspector]
#pragma warning disable CS0414 // Field is assigned but its value is never used
        private bool _lock;
#pragma warning restore CS0414 // Field is assigned but its value is never used

        [SerializeField] [HideInInspector]
        private string _path;

        #endregion

        #region ================================ METHODS

        [BoxGroup("Buttons", false, order: -99999)] [VerticalGroup("Buttons/Vertical")] [HorizontalGroup("Buttons/Vertical/Horizontal1")]
        [Button("Export", ButtonSizes.Medium)] [DisableIf("_lock")] [PropertyOrder(3)]
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

            var methodInfo = typeof(TConfig).GetMethod("OnSave", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            methodInfo?.Invoke(_config, Array.Empty<object>());
            exportConfig.Save(_config);

            if (string.IsNullOrEmpty(_assetID))
                _assetID = Guid.NewGuid().ToString();

            var assetPath = _path.Replace(_exportSettings.Extension, ASSET_EXTENSION);
            TConfigAsset container;
            if (File.Exists(assetPath))
            {
                container = AssetDatabase.LoadAssetAtPath<TConfigAsset>(assetPath);
                container.ID = _assetID;
            }
            else
            {
                container = CreateInstance<TConfigAsset>();
                container.ID = _assetID;
                AssetDatabase.CreateAsset(container, assetPath);
            }

            var configField = typeof(DW4ConfigAsset<TConfig>).GetField("_config", BindingFlags.Instance | BindingFlags.NonPublic);
            configField?.SetValue(container, _config);
            filedInfo?.SetValue(DW4.AppConfig, storageLogEnable);

            Save();
        }

        [BoxGroup("Buttons", false, order: -99999)] [VerticalGroup("Buttons/Vertical")] [HorizontalGroup("Buttons/Vertical/Horizontal1")]
        [Button("Import", ButtonSizes.Medium)] [DisableIf("_lock")] [PropertyOrder(2)]
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

            var response = importConfig.Load<TConfig>();
            if (response.Success)
                _config = response.Data;

            Save();

            filedInfo?.SetValue(DW4.AppConfig, storageLogEnable);
        }

        [BoxGroup("Buttons", false, order: -99999)] [VerticalGroup("Buttons/Vertical")] [HorizontalGroup("Buttons/Vertical/Horizontal1")]
        [Button("Lock", ButtonSizes.Medium)] [HideIf("_lock")] [PropertyOrder(5)]
        public virtual void Lock()
        {
            _lock = true;
            DisableSaveButton = true;
        }

        [BoxGroup("Buttons", false, order: -99999)] [VerticalGroup("Buttons/Vertical")] [HorizontalGroup("Buttons/Vertical/Horizontal1")]
        [Button("Reset ID", ButtonSizes.Medium)] [DisableIf("_lock")] [PropertyOrder(4)]
        public virtual void ResetID()
        {
            _assetID = string.Empty;
        }

        [BoxGroup("Buttons", false, order: -99999)] [VerticalGroup("Buttons/Vertical")] [HorizontalGroup("Buttons/Vertical/Horizontal1")]
        [Button("Unlock", ButtonSizes.Medium)] [ShowIf("_lock")] [PropertyOrder(5)]
        public virtual void Unlock()
        {
            _lock = false;
            DisableSaveButton = false;
        }

        #endregion
    }
}

#endif
