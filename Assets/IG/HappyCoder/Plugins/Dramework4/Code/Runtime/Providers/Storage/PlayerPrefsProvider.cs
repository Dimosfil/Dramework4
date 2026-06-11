using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

using Cysharp.Threading.Tasks;

using IG.HappyCoder.Plugins.Dramework4.Runtime.Configs;
using IG.HappyCoder.Plugins.Dramework4.Runtime.Constants;
using IG.HappyCoder.Plugins.Dramework4.Runtime.Data;

using Unity.Burst;
using Unity.IL2CPP.CompilerServices;


namespace IG.HappyCoder.Plugins.Dramework4.Runtime.Providers.Storage
{
    [BurstCompile]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    [SuppressMessage("ReSharper", "UnusedType.Global")]
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    internal static partial class PlayerPrefsProvider
    {
        #region ================================ METHODS

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static void Dispose()
        {
            if (_cts?.IsCancellationRequested == false)
                _cts?.Cancel();
            _cts?.Dispose();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static StorageResponse<T> Load<T>(StorageDataConfig config)
        {
            var type = typeof(T);
            if (type == typeof(float))
            {
                switch (config.EncryptionType)
                {
                    case EncryptionType.Xor:
                    case EncryptionType.Aes:
                    case EncryptionType.Md5:
                        return DecryptAndDeserialize<T>(config);
                    default:
                        return GetFloat<T>(config);
                }
            }

            if (type == typeof(int))
            {
                switch (config.EncryptionType)
                {
                    case EncryptionType.Xor:
                    case EncryptionType.Aes:
                    case EncryptionType.Md5:
                        return DecryptAndDeserialize<T>(config);
                    default:
                        return GetInt<T>(config);
                }
            }

            if (type == typeof(string))
            {
                switch (config.EncryptionType)
                {
                    case EncryptionType.Xor:
                    case EncryptionType.Aes:
                    case EncryptionType.Md5:
                        return DecryptAndDeserialize<T>(config);
                    default:
                        return GetString<T>(config);
                }
            }

            return DecryptAndDeserialize<T>(config);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static async UniTask<StorageResponse<T>> LoadAsync<T>(StorageDataConfig config)
        {
            await UniTask.Yield(_cts.Token);
            return Load<T>(config);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static void Save<T>(T data, StorageDataConfig config)
        {
            if (data is float floatData)
            {
                switch (config.EncryptionType)
                {
                    case EncryptionType.Xor:
                    case EncryptionType.Aes:
                    case EncryptionType.Md5:
                        SerializeAndEncrypt(floatData, config);
                        break;
                    default:
                        SetFloat(floatData, config);
                        break;
                }
            }
            else if (data is int intData)
            {
                switch (config.EncryptionType)
                {
                    case EncryptionType.Xor:
                    case EncryptionType.Aes:
                    case EncryptionType.Md5:
                        SerializeAndEncrypt(intData, config);
                        break;
                    default:
                        SetInt(intData, config);
                        break;
                }
            }
            else if (data is string stringData)
            {
                switch (config.EncryptionType)
                {
                    case EncryptionType.Xor:
                    case EncryptionType.Aes:
                    case EncryptionType.Md5:
                        SerializeAndEncrypt(stringData, config);
                        break;
                    default:
                        SetString(stringData, config);
                        break;
                }
            }
            else
            {
                SerializeAndEncrypt(data, config);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static async UniTask SaveAsync<T>(T data, StorageDataConfig config)
        {
            Save(data, config);
            await UniTask.Yield(_cts.Token);
        }

        #endregion
    }
}