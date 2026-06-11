using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;

using Cysharp.Threading.Tasks;

using IG.HappyCoder.Plugins.Dramework4.Runtime.Configs;
using IG.HappyCoder.Plugins.Dramework4.Runtime.Constants;
using IG.HappyCoder.Plugins.Dramework4.Runtime.Data;
using IG.HappyCoder.Plugins.Dramework4.Runtime.Providers.Encryption;
using IG.HappyCoder.Plugins.Dramework4.Runtime.Tools;

using MemoryPack;

using Newtonsoft.Json;

using Unity.IL2CPP.CompilerServices;


namespace IG.HappyCoder.Plugins.Dramework4.Runtime.Providers.Storage
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    [SuppressMessage("ReSharper", "UnusedType.Global")]
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    internal static partial class FileProvider
    {
        #region ================================ FIELDS

        private static readonly CancellationTokenSource _cts = new CancellationTokenSource();

        #endregion

        #region ================================ PROPERTIES AND INDEXERS

        private static string TAG => $"[{nameof(FileProvider)}] :";

        #endregion

        #region ================================ METHODS

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static byte[] Decrypt(byte[] bytes, StorageDataConfig config)
        {
            try
            {
                switch (config.EncryptionType)
                {
                    case EncryptionType.Xor:
                        if (DW4.AppConfig.LogStorage)
                            DW4.Log($"{TAG} Xor decrypting file: \"{config.FilePath}\". Config ID: \"{config.ID}\"");
                        XorProvider.Xor(ref bytes, config.XorKey);
                        break;
                    case EncryptionType.Aes:
                        if (DW4.AppConfig.LogStorage)
                            DW4.Log($"{TAG} Aes decrypting file: \"{config.FilePath}\". Config ID: \"{config.ID}\"");
                        bytes = AesProvider.Decrypt(bytes, config.AesKey, config.AesIV);
                        break;
                    case EncryptionType.Md5:
                        if (DW4.AppConfig.LogStorage)
                            DW4.Log($"{TAG} Md5 decrypting file: \"{config.FilePath}\". Config ID: \"{config.ID}\"");
                        bytes = MD5Provider.Decrypt(bytes, config.MD5Key);
                        break;
                }

                return bytes;
            }
            catch (Exception e)
            {
                throw new Exception($"Decrypting file: \"{config.FilePath}\" error. Config ID: \"{config.ID}\". {e.Message}");
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static StorageResponse<T> DecryptAndDeserializeBinary<T>(byte[] bytes, StorageDataConfig config)
        {
            bytes = Decrypt(bytes, config);
            var data = DeserializeBinary<T>(bytes, config);
            return new StorageResponse<T>(true, data);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static StorageResponse<T> DecryptAndDeserializeJson<T>(byte[] bytes, StorageDataConfig config)
        {
            bytes = Decrypt(bytes, config);
            var data = DeserializeJson<T>(Encoding.UTF8.GetString(bytes), config);
            return new StorageResponse<T>(true, data);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static T DeserializeBinary<T>(byte[] bytes, StorageDataConfig config)
        {
            try
            {
                if (DW4.AppConfig.LogStorage)
                    DW4.Log($"{TAG} Binary deserialize file: \"{config.FilePath}\". Config ID: \"{config.ID}\"");
                return MemoryPackSerializer.Deserialize<T>(bytes);
            }
            catch (Exception e)
            {
                throw new Exception($"{TAG} Binary deserialize file: \"{config.FilePath}\" error. Config ID: \"{config.ID}\". {e.Message}");
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static T DeserializeJson<T>(string json, StorageDataConfig config)
        {
            try
            {
                if (DW4.AppConfig.LogStorage)
                    DW4.Log($"{TAG} Json deserialize file: \"{config.FilePath}\". Config ID: \"{config.ID}\"");
                var data = JsonConvert.DeserializeObject<T>(json);
                return data;
            }
            catch (Exception e)
            {
                throw new Exception($"{TAG} Json deserialize file: \"{config.FilePath}\" error. Config ID: \"{config.ID}\". {e.Message}");
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static byte[] Encrypt(byte[] bytes, StorageDataConfig config)
        {
            try
            {
                switch (config.EncryptionType)
                {
                    case EncryptionType.Xor:
                        if (DW4.AppConfig.LogStorage)
                            DW4.Log($"{TAG} Xor encrypting file: \"{config.FilePath}\". Config ID: \"{config.ID}\"");
                        XorProvider.Xor(ref bytes, config.XorKey);
                        break;
                    case EncryptionType.Aes:
                        if (DW4.AppConfig.LogStorage)
                            DW4.Log($"{TAG} Aes encrypting file: \"{config.FilePath}\". Config ID: \"{config.ID}\"");
                        bytes = AesProvider.Encrypt(bytes, config.AesKey, config.AesIV);
                        break;
                    case EncryptionType.Md5:
                        if (DW4.AppConfig.LogStorage)
                            DW4.Log($"{TAG} Md5 encrypting file: \"{config.FilePath}\". Config ID: \"{config.ID}\"");
                        bytes = MD5Provider.Encrypt(bytes, config.MD5Key);
                        break;
                }

                return bytes;
            }
            catch (Exception e)
            {
                throw new Exception($"Encrypting file: \"{config.FilePath}\" error. Config ID: \"{config.ID}\". {e.Message}");
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static StorageResponse<T> LoadBinary<T>(StorageDataConfig config)
        {
            try
            {
                if (DW4.AppConfig.LogStorage)
                    DW4.Log($"{TAG} Sync loading binary file: \"{config.FilePath}\". Config ID: \"{config.ID}\"");
                var bytes = File.ReadAllBytes(config.FilePath);
                return DecryptAndDeserializeBinary<T>(bytes, config);
            }
            catch (Exception e)
            {
                throw new Exception($"{TAG} Sync loading binary file: \"{config.FilePath}\" error. Config ID: \"{config.ID}\". {e.Message}");
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static async UniTask<StorageResponse<T>> LoadBinaryAsync<T>(StorageDataConfig config)
        {
            try
            {
                if (DW4.AppConfig.LogStorage)
                    DW4.Log($"{TAG} Async loading binary file: \"{config.FilePath}\". Config ID: \"{config.ID}\"");
                var bytes = await File.ReadAllBytesAsync(config.FilePath, _cts.Token);
                return DecryptAndDeserializeBinary<T>(bytes, config);
            }
            catch (Exception e)
            {
                throw new Exception($"{TAG} Async loading binary file: \"{config.FilePath}\" error. Config ID: \"{config.ID}\". {e.Message}");
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        private static StorageResponse<T> LoadJson<T>(StorageDataConfig config)
        {
            try
            {
                if (config.EncryptionType == EncryptionType.None)
                {
                    if (DW4.AppConfig.LogStorage)
                        DW4.Log($"{TAG} Sync loading json file: \"{config.FilePath}\". Config ID: \"{config.ID}\"");
                    var json = File.ReadAllText(config.FilePath);
                    var data = DeserializeJson<T>(json, config);
                    return new StorageResponse<T>(true, data);
                }

                if (DW4.AppConfig.LogStorage)
                    DW4.Log($"{TAG} Sync loading encrypted json file: \"{config.FilePath}\". Config ID: \"{config.ID}\"");
                var bytes = File.ReadAllBytes(config.FilePath);
                return DecryptAndDeserializeJson<T>(bytes, config);
            }
            catch (Exception e)
            {
                throw new Exception($"{TAG} Sync loading json file: \"{config.FilePath}\" error. Config ID: \"{config.ID}\". {e.Message}");
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static async UniTask<StorageResponse<T>> LoadJsonAsync<T>(StorageDataConfig config)
        {
            try
            {
                if (config.EncryptionType == EncryptionType.None)
                {
                    if (DW4.AppConfig.LogStorage)
                        DW4.Log($"{TAG} Async loading json file: \"{config.FilePath}\". Config ID: \"{config.ID}\"");
                    var json = await File.ReadAllTextAsync(config.FilePath, _cts.Token);
                    var data = DeserializeJson<T>(json, config);
                    return new StorageResponse<T>(true, data);
                }

                if (DW4.AppConfig.LogStorage)
                    DW4.Log($"{TAG} Async loading encrypted json file: \"{config.FilePath}\". Config ID: \"{config.ID}\"");
                var bytes = await File.ReadAllBytesAsync(config.FilePath, _cts.Token);
                return DecryptAndDeserializeJson<T>(bytes, config);
            }
            catch (Exception e)
            {
                throw new Exception($"{TAG} Async loading json file: \"{config.FilePath}\" error. Config ID: \"{config.ID}\". {e.Message}");
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void SaveBinary<T>(T data, StorageDataConfig config)
        {
            try
            {
                var bytes = SerializeAndEncryptBinary(data, config);
                if (DW4.AppConfig.LogStorage)
                    DW4.Log($"{TAG} Sync saving binary file: \"{config.FilePath}\". Config ID: \"{config.ID}\"");
                File.WriteAllBytes(config.FilePath, bytes);
            }
            catch (Exception e)
            {
                throw new Exception($"{TAG} Sync saving binary file: \"{config.FilePath}\" error. Config ID: \"{config.ID}\". {e.Message}");
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static async UniTask SaveBinaryAsync<T>(T data, StorageDataConfig config)
        {
            try
            {
                var bytes = SerializeAndEncryptBinary(data, config);
                if (DW4.AppConfig.LogStorage)
                    DW4.Log($"{TAG} Async saving binary file: \"{config.FilePath}\". Config ID: \"{config.ID}\"");
                await File.WriteAllBytesAsync(config.FilePath, bytes);
            }
            catch (Exception e)
            {
                throw new Exception($"{TAG} Async saving binary file: \"{config.FilePath}\" error. Config ID: \"{config.ID}\". {e.Message}");
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void SaveJson<T>(T data, StorageDataConfig config)
        {
            try
            {
                var json = SerializeJson(data, config);

                if (config.EncryptionType == EncryptionType.None)
                {
                    if (DW4.AppConfig.LogStorage)
                        DW4.Log($"{TAG} Sync saving json file: \"{config.FilePath}\". Config ID: \"{config.ID}\"");
                    File.WriteAllText(config.FilePath, json);
                }
                else
                {
                    var bytes = Encrypt(Encoding.UTF8.GetBytes(json), config);
                    if (DW4.AppConfig.LogStorage)
                        DW4.Log($"{TAG} Sync saving encrypted json file: \"{config.FilePath}\". Config ID: \"{config.ID}\"");
                    File.WriteAllBytes(config.FilePath, bytes);
                }
            }
            catch (Exception e)
            {
                throw new Exception($"{TAG} Sync saving json file: \"{config.FilePath}\" error. Config ID: \"{config.ID}\". {e.Message}");
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static async UniTask SaveJsonAsync<T>(T data, StorageDataConfig config)
        {
            try
            {
                var json = SerializeJson(data, config);

                if (config.EncryptionType == EncryptionType.None)
                {
                    if (DW4.AppConfig.LogStorage)
                        DW4.Log($"{TAG} Async saving json file: \"{config.FilePath}\". Config ID: \"{config.ID}\"");
                    await File.WriteAllTextAsync(config.FilePath, json);
                }
                else
                {
                    var bytes = Encrypt(Encoding.UTF8.GetBytes(json), config);
                    if (DW4.AppConfig.LogStorage)
                        DW4.Log($"{TAG} Async saving encrypted json file: \"{config.FilePath}\". Config ID: \"{config.ID}\"");
                    await File.WriteAllBytesAsync(config.FilePath, bytes);
                }
            }
            catch (Exception e)
            {
                throw new Exception($"{TAG} Async saving json file: \"{config.FilePath}\" error. Config ID: \"{config.ID}\". {e.Message}");
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static byte[] SerializeAndEncryptBinary<T>(T data, StorageDataConfig config)
        {
            var bytes = SerializeBinary(data, config);
            return Encrypt(bytes, config);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static byte[] SerializeAndEncryptJson<T>(T data, StorageDataConfig config)
        {
            var json = SerializeJson(data, config);
            return Encrypt(Encoding.UTF8.GetBytes(json), config);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static byte[] SerializeBinary<T>(T data, StorageDataConfig config)
        {
            try
            {
                if (DW4.AppConfig.LogStorage)
                    DW4.Log($"{TAG} Binary serialize file: \"{config.FilePath}\". Config ID: \"{config.ID}\"");
                return MemoryPackSerializer.Serialize(data);
            }
            catch (Exception e)
            {
                throw new Exception($"{TAG} Binary serialize file: \"{config.FilePath}\" error. Config ID: \"{config.ID}\". {e.Message}");
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static string SerializeJson<T>(T data, StorageDataConfig config)
        {
            try
            {
                if (DW4.AppConfig.LogStorage)
                    DW4.Log($"{TAG} Json serialize file: \"{config.FilePath}\". Config ID: \"{config.ID}\"");
                return JsonConvert.SerializeObject(data, Formatting.Indented);
            }
            catch (Exception e)
            {
                throw new Exception($"{TAG} Json serialize file: \"{config.FilePath}\" error. Config ID: \"{config.ID}\". {e.Message}");
            }
        }

        #endregion
    }
}