using System;

using IG.HappyCoder.Plugins.Dramework4.Runtime.Constants;
using IG.HappyCoder.Plugins.Dramework4.Runtime.Tools;

using Sirenix.OdinInspector;

using UnityEngine;


namespace IG.HappyCoder.Plugins.Dramework4.Runtime.Configurators
{
    [Serializable]
    internal class ConfigExportSettings
    {
        #region ================================ FIELDS

        private const int LABEL_WIDTH = 110;

        [FoldoutGroup("Export Settings", -99998)] [BoxGroup("Export Settings/Extension", false)]
        [LabelWidth(LABEL_WIDTH)] [LabelText("Extension:")]
        [SerializeField]
        private string _extension = "json";

        [FoldoutGroup("Export Settings", -99998)] [BoxGroup("Export Settings/Serialization Type", false)]
        [LabelWidth(LABEL_WIDTH)] [LabelText("Serialization Type:")]
        [SerializeField]
        private SerializationType _serializationType;

        [FoldoutGroup("Export Settings", -99998)] [BoxGroup("Export Settings/Encryption Type", false)]
        [LabelWidth(LABEL_WIDTH)] [LabelText("Encryption Type:")]
        [SerializeField]
        private EncryptionType _encryptionType;

        [FoldoutGroup("Export Settings", -99998)] [BoxGroup("Export Settings/Xor Key", false)] [HorizontalGroup("Export Settings/Xor Key/Horizontal")]
        [LabelWidth(60)] [LabelText("Xor Key:")]
        [SerializeField] [ShowIf(nameof(IsXor))] [ReadOnly]
        private byte _xorKey;

        [FoldoutGroup("Export Settings", -99998)] [BoxGroup("Export Settings/Aes Key", false)] [HorizontalGroup("Export Settings/Aes Key/Horizontal")]
        [LabelWidth(60)] [LabelText("AES Key:")]
        [SerializeField] [ShowIf(nameof(IsAes))] [ReadOnly]
        private string _aesKey;

        [FoldoutGroup("Export Settings", -99998)] [BoxGroup("Export Settings/Aes IV", false)] [HorizontalGroup("Export Settings/Aes IV/Horizontal")]
        [LabelWidth(60)] [LabelText("AES IV:")]
        [SerializeField] [ShowIf(nameof(IsAes))] [ReadOnly]
        private string _aesIV;

        [FoldoutGroup("Export Settings", -99998)] [BoxGroup("Export Settings/MD5 Key", false)] [HorizontalGroup("Export Settings/MD5 Key/Horizontal")]
        [LabelWidth(60)] [LabelText("MD5 Key:")]
        [SerializeField] [ShowIf(nameof(IsMd5))] [ReadOnly]
        private string _md5Key;

        #endregion

        #region ================================ PROPERTIES AND INDEXERS

        internal string AesIV => _aesIV;
        internal string AesKey => _aesKey;
        internal EncryptionType EncryptionType => _encryptionType;
        internal string Extension => _extension;
        internal string MD5Key => _md5Key;
        internal SerializationType SerializationType => _serializationType;
        internal byte XorKey => _xorKey;
        private bool IsAes => _encryptionType == EncryptionType.Aes;
        private bool IsMd5 => _encryptionType == EncryptionType.Md5;
        private bool IsXor => _encryptionType == EncryptionType.Xor;

        #endregion

        #region ================================ METHODS

        [FoldoutGroup("Export Settings", -99998)] [BoxGroup("Export Settings/Aes IV", false)] [HorizontalGroup("Export Settings/Aes IV/Horizontal", 64, MarginLeft = 4)]
        [Button("Generate", 21)] [ShowIf("IsAes")]
        private void GenerateAesIV()
        {
            _aesIV = DW4.StringTools.GetRandomString(16);
        }

        [FoldoutGroup("Export Settings", -99998)] [BoxGroup("Export Settings/Aes Key", false)] [HorizontalGroup("Export Settings/Aes Key/Horizontal", 64, MarginLeft = 4)]
        [Button("Generate", 21)] [ShowIf("IsAes")]
        private void GenerateAesKey()
        {
            _aesKey = DW4.StringTools.GetRandomString(32);
        }

        [FoldoutGroup("Export Settings", -99998)] [BoxGroup("Export Settings/MD5 Key", false)] [HorizontalGroup("Export Settings/MD5 Key/Horizontal", 64, MarginLeft = 4)]
        [Button("Generate", 21)] [ShowIf("IsMd5")]
        private void GenerateMd5Key()
        {
            _md5Key = DW4.StringTools.GetRandomString(16);
        }

        [FoldoutGroup("Export Settings", -99998)] [BoxGroup("Export Settings/Xor Key", false)] [HorizontalGroup("Export Settings/Xor Key/Horizontal", 64, MarginLeft = 4)]
        [Button("Generate", 21)] [ShowIf("IsXor")]
        private void GenerateXorKey()
        {
            _xorKey = (byte)DW4.GetRandom(1, 8);
        }

        #endregion
    }
}