using System;
using System.Runtime.CompilerServices;

using Cysharp.Threading.Tasks;

using IG.HappyCoder.Plugins.Dramework4.Runtime.Constants;
using IG.HappyCoder.Plugins.Dramework4.Runtime.Data;
using IG.HappyCoder.Plugins.Dramework4.Runtime.Tools;

using Newtonsoft.Json;

using Sirenix.OdinInspector;

using UnityEngine;


namespace IG.HappyCoder.Plugins.Dramework4.Runtime.Configs
{
    [Serializable]
    public class StorageDataConfig
    {
        #region ================================ FIELDS

        private const int LABEL_WIDTH = 110;

        [JsonProperty]
        [FoldoutGroup("Config")] [BoxGroup("Config/ID", false)]
        [LabelWidth(LABEL_WIDTH)] [LabelText("ID:")]
        [SerializeField]
        private string _id;

        [JsonProperty]
        [FoldoutGroup("Config")] [BoxGroup("Config/Storage Type", false)]
        [LabelWidth(LABEL_WIDTH)] [LabelText("Storage Type:")]
        [SerializeField]
        private StorageType _storageType;

        [JsonProperty]
        [FoldoutGroup("Config")] [BoxGroup("Config/Key", false)]
        [LabelWidth(LABEL_WIDTH)] [LabelText("Key:")]
        [SerializeField] [ShowIf("IsPlayerPrefs")]
        private string _playerPrefsKey;

        [JsonProperty]
        [FoldoutGroup("Config")] [BoxGroup("Config/Path", false)]
        [LabelWidth(LABEL_WIDTH)] [LabelText("Path:")]
        [SerializeField] [ShowIf("IsFile")]
        private string _filePath;

        [JsonProperty]
        [FoldoutGroup("Config")] [BoxGroup("Config/Remote URL", false)]
        [LabelWidth(LABEL_WIDTH)] [LabelText("URL:")]
        [SerializeField] [ShowIf("IsRemote")]
        private string _remoteURL;

        [JsonProperty]
        [FoldoutGroup("Config")] [BoxGroup("Config/Serialization Type", false)]
        [LabelWidth(LABEL_WIDTH)] [LabelText("Serialization Type:")]
        [SerializeField]
        private SerializationType _serializationType;

        [JsonProperty]
        [FoldoutGroup("Config")] [BoxGroup("Config/Encryption Type", false)]
        [LabelWidth(LABEL_WIDTH)] [LabelText("Encryption Type:")]
        [SerializeField]
        private EncryptionType _encryptionType;

        [JsonProperty]
        [FoldoutGroup("Config")] [BoxGroup("Config/Xor Key", false)] [HorizontalGroup("Config/Xor Key/Horizontal")]
        [LabelWidth(60)] [LabelText("Xor Key:")]
        [SerializeField] [ShowIf("IsXor")] [ReadOnly]
        private byte _xorKey;

        [JsonProperty]
        [FoldoutGroup("Config")] [BoxGroup("Config/AES Key", false)] [HorizontalGroup("Config/AES Key/Horizontal")]
        [LabelWidth(60)] [LabelText("AES Key:")]
        [SerializeField] [ShowIf("IsAes")] [ReadOnly]
        private string _aesKey;

        [JsonProperty]
        [FoldoutGroup("Config")] [BoxGroup("Config/AES IV", false)] [HorizontalGroup("Config/AES IV/Horizontal")]
        [LabelWidth(60)] [LabelText("AES IV:")]
        [SerializeField] [ShowIf("IsAes")] [ReadOnly]
        private string _aesIV;

        [JsonProperty]
        [FoldoutGroup("Config")] [BoxGroup("Config/MD5 Key", false)] [HorizontalGroup("Config/MD5 Key/Horizontal")]
        [LabelWidth(60)] [LabelText("MD5 Key:")]
        [SerializeField] [ShowIf("IsMd5")] [ReadOnly]
        private string _md5Key;

        #endregion

        #region ================================ CONSTRUCTORS AND DESTRUCTOR

        public StorageDataConfig(
            string id,
            string filePath,
            StorageType storageType,
            SerializationType serializationType,
            EncryptionType encryptionType = EncryptionType.None,
            byte xorKey = 0,
            string aesKey = "",
            string aesIv = "",
            string md5Key = "")
        {
            _id = id;
            _filePath = filePath;
            _storageType = storageType;
            _serializationType = serializationType;
            _encryptionType = encryptionType;
            _xorKey = xorKey;
            _aesKey = aesKey;
            _aesIV = aesIv;
            _md5Key = md5Key;
        }

        #endregion

        #region ================================ PROPERTIES AND INDEXERS

        [JsonIgnore]
        public string AesIV => _aesIV;
        [JsonIgnore]
        public string AesKey => _aesKey;
        [JsonIgnore]
        public EncryptionType EncryptionType => _encryptionType;
        [JsonIgnore]
        public string FilePath => _filePath;
        [JsonIgnore]
        public string ID => _id;
        [JsonIgnore]
        public string MD5Key => _md5Key;
        [JsonIgnore]
        public string PlayerPrefsKey => _playerPrefsKey;
        [JsonIgnore]
        public string RemoteURL => _remoteURL;
        [JsonIgnore]
        public SerializationType SerializationType => _serializationType;
        [JsonIgnore]
        public StorageType StorageType => _storageType;
        [JsonIgnore]
        public byte XorKey => _xorKey;

        private bool IsAes => _encryptionType == EncryptionType.Aes;
        private bool IsFile => _storageType == StorageType.File;
        private bool IsMd5 => _encryptionType == EncryptionType.Md5;
        private bool IsPlayerPrefs => _storageType == StorageType.PlayerPrefs;
        private bool IsRemote => _storageType == StorageType.Remote;
        private bool IsXor => _encryptionType == EncryptionType.Xor;

        #endregion

        #region ================================ METHODS

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public StorageResponse<T> Load<T>()
        {
            return DW4.Load<T>(this);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public async UniTask<StorageResponse<T>> LoadAsync<T>(Action<float> onProgress = null)
        {
            return await DW4.LoadAsync<T>(this, onProgress);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Save<T>(T data)
        {
            DW4.Save(data, this);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public async UniTask SaveAsync<T>(T data)
        {
            await DW4.SaveAsync(data, this);
        }

        [FoldoutGroup("Config")] [BoxGroup("Config/AES IV", false)] [HorizontalGroup("Config/AES IV/Horizontal", 64, MarginLeft = 4)]
        [Button("Generate", 21)] [ShowIf("IsAes")]
        private void GenerateAesIV()
        {
            _aesIV = DW4.StringTools.GetRandomString(16);
        }

        [FoldoutGroup("Config")] [BoxGroup("Config/AES Key", false)] [HorizontalGroup("Config/AES Key/Horizontal", 64, MarginLeft = 4)]
        [Button("Generate", 21)] [ShowIf("IsAes")]
        private void GenerateAesKey()
        {
            _aesKey = DW4.StringTools.GetRandomString(32);
        }

        [FoldoutGroup("Config")] [BoxGroup("Config/MD5 Key", false)] [HorizontalGroup("Config/MD5 Key/Horizontal", 64, MarginLeft = 4)]
        [Button("Generate", 21)] [ShowIf("IsMd5")]
        private void GenerateMd5Key()
        {
            _md5Key = DW4.StringTools.GetRandomString(16);
        }

        [FoldoutGroup("Config")] [BoxGroup("Config/Xor Key", false)] [HorizontalGroup("Config/Xor Key/Horizontal", 64, MarginLeft = 4)]
        [Button("Generate", 21)] [ShowIf("IsXor")]
        private void GenerateXorKey()
        {
            _xorKey = (byte)DW4.GetRandom(1, 8);
        }

        #endregion
    }
}