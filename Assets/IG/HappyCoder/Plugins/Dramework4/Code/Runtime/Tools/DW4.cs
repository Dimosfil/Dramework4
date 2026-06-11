using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

using IG.HappyCoder.Plugins.Dramework4.Runtime.Attributes.Initialization;
using IG.HappyCoder.Plugins.Dramework4.Runtime.Configs;
using IG.HappyCoder.Plugins.Dramework4.Runtime.Core;
using IG.HappyCoder.Plugins.Dramework4.Runtime.Providers.Storage;

using Unity.Burst;
using Unity.IL2CPP.CompilerServices;

using UnityEngine;


namespace IG.HappyCoder.Plugins.Dramework4.Runtime.Tools
{
    [BurstCompile]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    [SuppressMessage("ReSharper", "UnusedType.Global")]
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public static partial class DW4
    {
        #region ================================ FIELDS

        [SetOnPlayModeStateChanged(false)]
        private static bool _lock;
        private static AppConfig _appConfig;

        #endregion

        #region ================================ PROPERTIES AND INDEXERS

        public static string ProjectRootFolder => AppConfig.ProjectRootFolder;

        internal static AppConfig AppConfig
        {
            get
            {
                if (_appConfig != null)
                    return _appConfig;

                return _appConfig = Resources.Load<AppConfig>(AppConfig.FILENAME);
            }
        }

        #endregion

        #region ================================ METHODS

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Lock()
        {
            if (_lock) return;
            Dispatcher.Instance.Lock();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void LockUpdate()
        {
            if (_lock) return;
            Dispatcher.Instance.LockUpdate();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Unlock()
        {
            if (_lock) return;
            Dispatcher.Instance.Unlock();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void UnlockUpdate()
        {
            if (_lock) return;
            Dispatcher.Instance.UnlockUpdate();
        }

        internal static void Dispose()
        {
            if (_lock) return;
            _lock = true;
            PlayerPrefsProvider.Dispose();
            FileProvider.Dispose();
            _lock = false;
        }

        #endregion
    }
}