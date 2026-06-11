using System.Diagnostics.CodeAnalysis;

using IG.HappyCoder.Plugins.Dramework4.Runtime.Constants;
using IG.HappyCoder.Plugins.Dramework4.Runtime.ScriptableObjects;

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
    public partial class StorageDataConfigAsset : DScriptableObject
    {
        #region ================================ FIELDS

        private const int LABEL_WIDTH = 110;

        [BoxGroup("ID", false)]
        [LabelWidth(LABEL_WIDTH)] [LabelText("ID:")]
        [SerializeField] [ReadOnly]
        private string _id;

        [BoxGroup("Storage Type", false)]
        [LabelWidth(LABEL_WIDTH)] [LabelText("Storage Type:")]
        [SerializeField] [ReadOnly]
        private StorageType _storageType;

        [BoxGroup("Key", false)]
        [LabelWidth(LABEL_WIDTH)] [LabelText("Key:")]
        [SerializeField] [ShowIf("IsPlayerPrefs")] [ReadOnly]
        private string _playerPrefsKey;

        [BoxGroup("Path", false)]
        [LabelWidth(LABEL_WIDTH)] [LabelText("Path:")]
        [SerializeField] [ShowIf("IsFile")] [ReadOnly]
        private string _filePath;

        [BoxGroup("Remote URL", false)]
        [LabelWidth(LABEL_WIDTH)] [LabelText("URL:")]
        [SerializeField] [ShowIf("IsRemote")] [ReadOnly]
        private string _remoteURL;

        [BoxGroup("Serialization Type", false)]
        [LabelWidth(LABEL_WIDTH)] [LabelText("Serialization Type:")]
        [SerializeField] [ReadOnly]
        private SerializationType _serializationType;

        [BoxGroup("Encryption Type", false)]
        [LabelWidth(LABEL_WIDTH)] [LabelText("Encryption Type:")]
        [SerializeField] [ReadOnly]
        private EncryptionType _encryptionType;

        [BoxGroup("Xor Key", false)] [HorizontalGroup("Xor Key/Horizontal")]
        [LabelWidth(60)] [LabelText("Xor Key:")]
        [SerializeField] [ShowIf("IsXor")] [ReadOnly]
        private byte _xorKey;

        [BoxGroup("AES Key", false)] [HorizontalGroup("AES Key/Horizontal")]
        [LabelWidth(60)] [LabelText("AES Key:")]
        [SerializeField] [ShowIf("IsAes")] [ReadOnly]
        private string _aesKey;

        [BoxGroup("AES IV", false)] [HorizontalGroup("AES IV/Horizontal")]
        [LabelWidth(60)] [LabelText("AES IV:")]
        [SerializeField] [ShowIf("IsAes")] [ReadOnly]
        private string _aesIV;

        [BoxGroup("MD5 Key", false)] [HorizontalGroup("MD5 Key/Horizontal")]
        [LabelWidth(60)] [LabelText("MD5 Key:")]
        [SerializeField] [ShowIf("IsMd5")] [ReadOnly]
        private string _md5Key;

        [SerializeField] [HideInInspector]
        private StorageDataConfig _config;

        #endregion

        #region ================================ PROPERTIES AND INDEXERS

        public StorageDataConfig Config => _config;

        private bool IsAes => _encryptionType == EncryptionType.Aes;
        private bool IsFile => _storageType == StorageType.File;
        private bool IsMd5 => _encryptionType == EncryptionType.Md5;
        private bool IsPlayerPrefs => _storageType == StorageType.PlayerPrefs;
        private bool IsRemote => _storageType == StorageType.Remote;
        private bool IsXor => _encryptionType == EncryptionType.Xor;

        #endregion

        #region ================================ METHODS

        private void Initialize(StorageDataConfig config)
        {
            _config = config;
            _id = config.ID;
            _storageType = config.StorageType;
            _playerPrefsKey = config.PlayerPrefsKey;
            _filePath = config.FilePath;
            _remoteURL = config.RemoteURL;
            _serializationType = config.SerializationType;
            _encryptionType = config.EncryptionType;
            _xorKey = config.XorKey;
            _aesKey = config.AesKey;
            _aesIV = config.AesIV;
            _md5Key = config.MD5Key;
        }

        #endregion
    }
}