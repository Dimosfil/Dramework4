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
            if (config.StorageType == StorageType.PlayerPrefs)
                return PlayerPrefsProvider.Load<T>(config);

            if (config.StorageType == StorageType.File)
                return FileProvider.Load<T>(config);

            throw new Exception($"Unknown storage type. Type: {config.StorageType}");
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static async UniTask<StorageResponse<T>> LoadAsync<T>(StorageDataConfig config, Action<float> onProgress)
        {
            if (config.StorageType == StorageType.PlayerPrefs)
                return await PlayerPrefsProvider.LoadAsync<T>(config);

            if (config.StorageType == StorageType.File)
                return await FileProvider.LoadAsync<T>(config);

            if (config.StorageType == StorageType.Remote)
                return await RemoteProvider.LoadAsync<T>(config, onProgress);

            throw new Exception($"Unknown storage type. Type: {config.StorageType}");
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Save<T>(T data, StorageDataConfig config)
        {
            if (config.StorageType == StorageType.PlayerPrefs)
                PlayerPrefsProvider.Save(data, config);
            else if (config.StorageType == StorageType.File)
                FileProvider.Save(data, config);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static async UniTask SaveAsync<T>(T data, StorageDataConfig config, Action<float> onProgress = null)
        {
            if (config.StorageType == StorageType.PlayerPrefs)
                await PlayerPrefsProvider.SaveAsync(data, config);
            else if (config.StorageType.HasFlag(StorageType.File))
                await FileProvider.SaveAsync(data, config);
        }

        #endregion
    }
}