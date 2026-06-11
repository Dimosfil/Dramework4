using System;
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
    internal static partial class FileProvider
    {
        #region ================================ METHODS

        internal static void Dispose()
        {
            if (_cts?.IsCancellationRequested == false)
                _cts?.Cancel();
            _cts?.Dispose();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static StorageResponse<T> Load<T>(StorageDataConfig config)
        {
            switch (config.SerializationType)
            {
                case SerializationType.JSON:
                {
                    return LoadJson<T>(config);
                }
                case SerializationType.Binary:
                {
                    return LoadBinary<T>(config);
                }
                default:
                    throw new ArgumentOutOfRangeException(nameof(config.SerializationType), config.SerializationType, null);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static async UniTask<StorageResponse<T>> LoadAsync<T>(StorageDataConfig config)
        {
            switch (config.SerializationType)
            {
                case SerializationType.JSON:
                {
                    return await LoadJsonAsync<T>(config);
                }
                case SerializationType.Binary:
                {
                    return await LoadBinaryAsync<T>(config);
                }
                default:
                    throw new ArgumentOutOfRangeException(nameof(config.SerializationType), config.SerializationType, null);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static void Save<T>(T data, StorageDataConfig config)
        {
            switch (config.SerializationType)
            {
                case SerializationType.JSON:
                    SaveJson(data, config);
                    break;
                case SerializationType.Binary:
                    SaveBinary(data, config);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(config.SerializationType), config.SerializationType, null);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static async UniTask SaveAsync<T>(T data, StorageDataConfig config)
        {
            switch (config.SerializationType)
            {
                case SerializationType.JSON:
                    await SaveJsonAsync(data, config);
                    break;
                case SerializationType.Binary:
                    await SaveBinaryAsync(data, config);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(config.SerializationType), config.SerializationType, null);
            }
        }

        #endregion
    }
}