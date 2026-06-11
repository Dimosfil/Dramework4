using System;
using System.Diagnostics.CodeAnalysis;
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

using Unity.Burst;
using Unity.IL2CPP.CompilerServices;

using UnityEngine.Networking;


namespace IG.HappyCoder.Plugins.Dramework4.Runtime.Providers.Storage
{
    [BurstCompile]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    [SuppressMessage("ReSharper", "UnusedType.Global")]
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    internal static class RemoteProvider
    {
        #region ================================ FIELDS

        private static readonly CancellationTokenSource _cts = new CancellationTokenSource();

        #endregion

        #region ================================ PROPERTIES AND INDEXERS

        private static string TAG => $"[{nameof(RemoteProvider)}] :";

        #endregion

        #region ================================ METHODS

        internal static void Dispose()
        {
            if (_cts.IsCancellationRequested == false)
                _cts?.Cancel();
            _cts?.Dispose();
        }

        internal static async UniTask<StorageResponse<T>> LoadAsync<T>(StorageDataConfig config, Action<float> onProgress)
        {
            try
            {
                if (DW4.AppConfig.LogStorage)
                    DW4.Log($"{TAG} Start loading remote file: \"{config.RemoteURL}\". Config ID: \"{config.ID}\"");

                var request = UnityWebRequest.Get(config.RemoteURL);
                var asyncOp = request.SendWebRequest();
                onProgress?.Invoke(0);
                while (asyncOp.isDone == false)
                {
                    onProgress?.Invoke(asyncOp.progress);
                    await UniTask.Yield(_cts.Token);
                }
                onProgress?.Invoke(1);

                if (request.result == UnityWebRequest.Result.Success)
                {
                    if (DW4.AppConfig.LogStorage)
                        DW4.Log($"{TAG} Loading remote file: \"{config.RemoteURL}\" complete. Config ID: \"{config.ID}\"");

                    var bytes = request.downloadHandler.data;
                    switch (config.EncryptionType)
                    {
                        case EncryptionType.Xor:
                            if (DW4.AppConfig.LogStorage)
                                DW4.Log($"{TAG} Xor decrypting remote file: \"{config.RemoteURL}\". Config ID: \"{config.ID}\"");
                            XorProvider.Xor(ref bytes, config.XorKey);
                            break;
                        case EncryptionType.Aes:
                            if (DW4.AppConfig.LogStorage)
                                DW4.Log($"{TAG} Aes decrypting remote file: \"{config.RemoteURL}\". Config ID: \"{config.ID}\"");
                            bytes = AesProvider.Decrypt(bytes, config.AesKey, config.AesIV);
                            break;
                        case EncryptionType.Md5:
                            if (DW4.AppConfig.LogStorage)
                                DW4.Log($"{TAG} Md5 decrypting remote file: \"{config.RemoteURL}\". Config ID: \"{config.ID}\"");
                            bytes = MD5Provider.Decrypt(bytes, config.MD5Key);
                            break;
                    }

                    switch (config.SerializationType)
                    {
                        case SerializationType.JSON:
                            if (DW4.AppConfig.LogStorage)
                                DW4.Log($"{TAG} Json deserialize remote file: \"{config.RemoteURL}\". Config ID: \"{config.ID}\"");
                            var result = JsonConvert.DeserializeObject<T>(Encoding.UTF8.GetString(bytes));
                            return new StorageResponse<T>(true, result);
                        case SerializationType.Binary:
                            if (DW4.AppConfig.LogStorage)
                                DW4.Log($"{TAG} Binary deserialize remote file: \"{config.RemoteURL}\". Config ID: \"{config.ID}\"");
                            result = MemoryPackSerializer.Deserialize<T>(request.downloadHandler.data);
                            return new StorageResponse<T>(true, result);
                    }
                }
                else
                {
                    DW4.LogError($"{TAG} Loading remote file: \"{config.RemoteURL}\" error. Config ID: \"{config.ID}\". {request.error}");
                }

                return new StorageResponse<T>(false, default);
            }
            catch (Exception e)
            {
                throw new Exception($"{TAG} Loading remote file: \"{config.RemoteURL}\" error. Config ID: \"{config.ID}\". {e.Message}");
            }
        }

        #endregion
    }
}