using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;

using IG.HappyCoder.Plugins.Dramework4.Runtime.Configs;
using IG.HappyCoder.Plugins.Dramework4.Runtime.Constants;
using IG.HappyCoder.Plugins.Dramework4.Runtime.Data;
using IG.HappyCoder.Plugins.Dramework4.Runtime.Providers.Encryption;
using IG.HappyCoder.Plugins.Dramework4.Runtime.Tools;

using MemoryPack;

using Newtonsoft.Json;

using Unity.IL2CPP.CompilerServices;

using UnityEngine;


namespace IG.HappyCoder.Plugins.Dramework4.Runtime.Providers.Storage
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    [SuppressMessage("ReSharper", "UnusedType.Global")]
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    internal static partial class PlayerPrefsProvider
    {
        #region ================================ FIELDS

        private static readonly CancellationTokenSource _cts = new CancellationTokenSource();

        #endregion

        #region ================================ PROPERTIES AND INDEXERS

        private static string TAG => $"[{nameof(PlayerPrefsProvider)}] :";

        #endregion

        #region ================================ METHODS

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static byte[] Decrypt(string data, StorageDataConfig config)
        {
            try
            {
                var bytes = Convert.FromBase64String(data);
                switch (config.EncryptionType)
                {
                    case EncryptionType.Xor:
                        if (DW4.AppConfig.LogStorage)
                            DW4.Log($"{TAG} Xor decrypting player prefs: \"{config.PlayerPrefsKey}\". Config ID: \"{config.ID}\"");
                        XorProvider.Xor(ref bytes, config.XorKey);
                        break;
                    case EncryptionType.Aes:
                        if (DW4.AppConfig.LogStorage)
                            DW4.Log($"{TAG} Aes decrypting player prefs: \"{config.PlayerPrefsKey}\". Config ID: \"{config.ID}\"");
                        bytes = AesProvider.Decrypt(bytes, config.AesKey, config.AesIV);
                        break;
                    case EncryptionType.Md5:
                        if (DW4.AppConfig.LogStorage)
                            DW4.Log($"{TAG} Md5 decrypting player prefs: \"{config.PlayerPrefsKey}\". Config ID: \"{config.ID}\"");
                        bytes = MD5Provider.Decrypt(bytes, config.MD5Key);
                        break;
                }
                return bytes;
            }
            catch (Exception e)
            {
                throw new Exception($"Decrypting player prefs: \"{config.PlayerPrefsKey}\" error. Config ID: \"{config.ID}\". {e.Message}");
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static StorageResponse<T> DecryptAndDeserialize<T>(StorageDataConfig config)
        {
            var data = GetString(config);
            var bytes = Decrypt(data, config);
            return Deserialize<T>(bytes, config);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static StorageResponse<T> Deserialize<T>(byte[] bytes, StorageDataConfig config)
        {
            try
            {
                switch (config.SerializationType)
                {
                    case SerializationType.JSON:
                        if (DW4.AppConfig.LogStorage)
                            DW4.Log($"{TAG} Json deserialize player prefs: \"{config.PlayerPrefsKey}\". Config ID: \"{config.ID}\"");
                        var result = JsonConvert.DeserializeObject<T>(Encoding.UTF8.GetString(bytes));
                        return new StorageResponse<T>(true, result);
                    case SerializationType.Binary:
                        if (DW4.AppConfig.LogStorage)
                            DW4.Log($"{TAG} Binary deserialize player prefs: \"{config.PlayerPrefsKey}\". Config ID: \"{config.ID}\"");
                        result = MemoryPackSerializer.Deserialize<T>(bytes);
                        return new StorageResponse<T>(true, result);
                    default:
                        return new StorageResponse<T>(false, default);
                }
            }
            catch (Exception e)
            {
                throw new Exception($"{TAG} Deserialize player prefs: \"{config.PlayerPrefsKey}\" error. Config ID: \"{config.ID}\". {e.Message}");
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static string Encrypt(byte[] bytes, StorageDataConfig config)
        {
            try
            {
                switch (config.EncryptionType)
                {
                    case EncryptionType.Xor:
                        if (DW4.AppConfig.LogStorage)
                            DW4.Log($"{TAG} Xor encrypting player prefs: \"{config.PlayerPrefsKey}\". Config ID: \"{config.ID}\"");
                        XorProvider.Xor(ref bytes, config.XorKey);
                        break;
                    case EncryptionType.Aes:
                        if (DW4.AppConfig.LogStorage)
                            DW4.Log($"{TAG} Aes encrypting player prefs: \"{config.PlayerPrefsKey}\". Config ID: \"{config.ID}\"");
                        bytes = AesProvider.Encrypt(bytes, config.AesKey, config.AesIV);
                        break;
                    case EncryptionType.Md5:
                        if (DW4.AppConfig.LogStorage)
                            DW4.Log($"{TAG} Md5 encrypting player prefs: \"{config.PlayerPrefsKey}\". Config ID: \"{config.ID}\"");
                        bytes = MD5Provider.Encrypt(bytes, config.MD5Key);
                        break;
                }

                return Convert.ToBase64String(bytes);
            }
            catch (Exception e)
            {
                throw new Exception($"Encrypting player prefs: \"{config.PlayerPrefsKey}\" error. Config ID: \"{config.ID}\". {e.Message}");
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static StorageResponse<T> GetFloat<T>(StorageDataConfig config)
        {
            try
            {
                if (DW4.AppConfig.LogStorage)
                    DW4.Log($"{TAG} Get float player prefs: \"{config.PlayerPrefsKey}\". Config ID: \"{config.ID}\"");
                object result = PlayerPrefs.GetFloat(config.PlayerPrefsKey);
                return new StorageResponse<T>(true, (T)result);
            }
            catch (Exception e)
            {
                throw new Exception($"{TAG} Get float player prefs: \"{config.PlayerPrefsKey}\" error. Config ID: \"{config.ID}\". {e.Message}");
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static StorageResponse<T> GetInt<T>(StorageDataConfig config)
        {
            try
            {
                if (DW4.AppConfig.LogStorage)
                    DW4.Log($"{TAG} Get int player prefs: \"{config.PlayerPrefsKey}\". Config ID: \"{config.ID}\"");
                object result = PlayerPrefs.GetInt(config.PlayerPrefsKey);
                return new StorageResponse<T>(true, (T)result);
            }
            catch (Exception e)
            {
                throw new Exception($"{TAG} Get int player prefs: \"{config.PlayerPrefsKey}\" error. Config ID: \"{config.ID}\". {e.Message}");
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static StorageResponse<T> GetString<T>(StorageDataConfig config)
        {
            try
            {
                if (DW4.AppConfig.LogStorage)
                    DW4.Log($"{TAG} Get string player prefs: \"{config.PlayerPrefsKey}\". Config ID: \"{config.ID}\"");
                object result = PlayerPrefs.GetString(config.PlayerPrefsKey);
                return new StorageResponse<T>(true, (T)result);
            }
            catch (Exception e)
            {
                throw new Exception($"{TAG} Get string player prefs: \"{config.PlayerPrefsKey}\" error. Config ID: \"{config.ID}\". {e.Message}");
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static string GetString(StorageDataConfig config)
        {
            try
            {
                if (DW4.AppConfig.LogStorage)
                    DW4.Log($"{TAG} Get string player prefs: \"{config.PlayerPrefsKey}\". Config ID: \"{config.ID}\"");
                return PlayerPrefs.GetString(config.PlayerPrefsKey);
            }
            catch (Exception e)
            {
                throw new Exception($"{TAG} Get string player prefs: \"{config.PlayerPrefsKey}\" error. Config ID: \"{config.ID}\". {e.Message}");
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static byte[] Serialize<T>(T data, StorageDataConfig config)
        {
            try
            {
                switch (config.SerializationType)
                {
                    case SerializationType.JSON:
                        if (DW4.AppConfig.LogStorage)
                            DW4.Log($"{TAG} Json serialize player prefs: \"{config.PlayerPrefsKey}\". Config ID: \"{config.ID}\"");
                        var json = JsonConvert.SerializeObject(data);
                        return Encoding.UTF8.GetBytes(json);
                    case SerializationType.Binary:
                        if (DW4.AppConfig.LogStorage)
                            DW4.Log($"{TAG} Binary serialize player prefs: \"{config.PlayerPrefsKey}\". Config ID: \"{config.ID}\"");
                        return MemoryPackSerializer.Serialize(data);
                    default:
                        return null;
                }
            }
            catch (Exception e)
            {
                throw new Exception($"{TAG} Serialize player prefs: \"{config.FilePath}\" error. Config ID: \"{config.ID}\". {e.Message}");
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void SerializeAndEncrypt<T>(T data, StorageDataConfig config)
        {
            var bytes = Serialize(data, config);
            var encrypted = Encrypt(bytes, config);
            SetString(encrypted, config);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void SetFloat(float value, StorageDataConfig config)
        {
            try
            {
                if (DW4.AppConfig.LogStorage)
                    DW4.Log($"{TAG} Set float player prefs: \"{config.PlayerPrefsKey}\". Config ID: \"{config.ID}\"");
                PlayerPrefs.SetFloat(config.PlayerPrefsKey, value);
            }
            catch (Exception e)
            {
                throw new Exception($"{TAG} Set float player prefs: \"{config.PlayerPrefsKey}\" error. Config ID: \"{config.ID}\". {e.Message}");
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void SetInt(int value, StorageDataConfig config)
        {
            try
            {
                if (DW4.AppConfig.LogStorage)
                    DW4.Log($"{TAG} Set int player prefs: \"{config.PlayerPrefsKey}\". Config ID: \"{config.ID}\"");
                PlayerPrefs.SetInt(config.PlayerPrefsKey, value);
            }
            catch (Exception e)
            {
                throw new Exception($"{TAG} Set int player prefs: \"{config.PlayerPrefsKey}\" error. Config ID: \"{config.ID}\". {e.Message}");
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void SetString(string value, StorageDataConfig config)
        {
            try
            {
                if (DW4.AppConfig.LogStorage)
                    DW4.Log($"{TAG} Set string player prefs: \"{config.PlayerPrefsKey}\". Config ID: \"{config.ID}\"");
                PlayerPrefs.SetString(config.PlayerPrefsKey, value);
            }
            catch (Exception e)
            {
                throw new Exception($"{TAG} Set string player prefs: \"{config.PlayerPrefsKey}\" error. Config ID: \"{config.ID}\". {e.Message}");
            }
        }

        #endregion
    }
}