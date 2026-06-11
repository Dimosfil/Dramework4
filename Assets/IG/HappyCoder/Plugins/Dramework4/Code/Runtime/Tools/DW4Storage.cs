using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

using Cysharp.Threading.Tasks;

using IG.HappyCoder.Plugins.Dramework4.Runtime.Configs;
using IG.HappyCoder.Plugins.Dramework4.Runtime.Constants;
using IG.HappyCoder.Plugins.Dramework4.Runtime.Data;
using IG.HappyCoder.Plugins.Dramework4.Runtime.Providers.Storage;

using Unity.IL2CPP.CompilerServices;


namespace IG.HappyCoder.Plugins.Dramework4.Runtime.Tools
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public static partial class DW4
    {
        #region ================================ METHODS

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static StorageResponse<T> Load<T>(StorageDataConfig config)
        {
            switch (config.StorageType)
            {
                case StorageType.PlayerPrefs:
                    return PlayerPrefsProvider.Load<T>(config);
                case StorageType.File:
                    return FileProvider.Load<T>(config);
                default:
                    throw UnknownStorageType(config.StorageType);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static async UniTask<StorageResponse<T>> LoadAsync<T>(StorageDataConfig config, Action<float> onProgress)
        {
            switch (config.StorageType)
            {
                case StorageType.PlayerPrefs:
                    return await PlayerPrefsProvider.LoadAsync<T>(config);
                case StorageType.File:
                    return await FileProvider.LoadAsync<T>(config);
                case StorageType.Remote:
                    return await RemoteProvider.LoadAsync<T>(config, onProgress);
                default:
                    throw UnknownStorageType(config.StorageType);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Save<T>(T data, StorageDataConfig config)
        {
            switch (config.StorageType)
            {
                case StorageType.PlayerPrefs:
                    PlayerPrefsProvider.Save(data, config);
                    break;
                case StorageType.File:
                    FileProvider.Save(data, config);
                    break;
                default:
                    throw UnknownStorageType(config.StorageType);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static async UniTask SaveAsync<T>(T data, StorageDataConfig config, Action<float> onProgress = null)
        {
            _ = onProgress;

            switch (config.StorageType)
            {
                case StorageType.PlayerPrefs:
                    await PlayerPrefsProvider.SaveAsync(data, config);
                    break;
                case StorageType.File:
                    await FileProvider.SaveAsync(data, config);
                    break;
                default:
                    throw UnknownStorageType(config.StorageType);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static ArgumentOutOfRangeException UnknownStorageType(StorageType storageType)
        {
            return new ArgumentOutOfRangeException(nameof(storageType), storageType, $"Unknown storage type. Type: {storageType}");
        }

        #endregion
    }
}
