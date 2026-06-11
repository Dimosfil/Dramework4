using System.Reflection;
using System.Runtime.CompilerServices;

using IG.HappyCoder.Plugins.Dramework4.Runtime.Tools;

using Unity.IL2CPP.CompilerServices;

using UnityEngine.SceneManagement;


namespace IG.HappyCoder.Plugins.Dramework4.Runtime.Core
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    internal partial class Dispatcher
    {
        #region ================================ METHODS

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static bool IsPluginAssembly(Assembly assembly)
        {
            return assembly.FullName.StartsWith("Sirenix")
                   || assembly.FullName.StartsWith("UniTask")
                   || assembly.FullName.StartsWith("LiteDB")
                   || assembly.FullName.StartsWith("TexturePackerImporter");
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static bool IsSystemAssembly(Assembly assembly)
        {
            return assembly.FullName.StartsWith("mscorlib")
                   || assembly.FullName.StartsWith("netstandard")
                   || assembly.FullName.StartsWith("System");
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static bool IsUnityAssembly(Assembly assembly)
        {
            return assembly.FullName.StartsWith("Mono")
                   || assembly.FullName.StartsWith("Bee")
                   || assembly.FullName.StartsWith("Unity")
                   || assembly.FullName.StartsWith("PlayerBuildProgramLibrary")
                   || assembly.FullName.StartsWith("ScriptCompilationBuildProgram")
                   || assembly.FullName.StartsWith("AndroidPlayerBuildProgram")
                   || assembly.FullName.StartsWith("nunit.framework")
                   || assembly.FullName.StartsWith("NiceIO")
                   || assembly.FullName.StartsWith("Anonymously Hosted DynamicMethods Assembly")
                   || assembly.FullName.StartsWith("ExCSS")
                   || assembly.FullName.StartsWith("JetBrains")
                   || assembly.FullName.StartsWith("ExCSS")
                   || assembly.FullName.StartsWith("Newtonsoft");
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool IsDW4Assembly(Assembly assembly)
        {
            return assembly.FullName.StartsWith("dramework")
                   || assembly.FullName.StartsWith("MemoryPack")
                   || assembly.FullName.StartsWith("ReportGeneratorMerged");
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool IsSceneLoaded(int buildIndex)
        {
            for (var i = 0; i < SceneManager.sceneCount; i++)
            {
                var scene = SceneManager.GetSceneAt(i);
                if (scene.buildIndex == buildIndex && scene.isLoaded)
                {
                    return true;
                }
            }
            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal void Lock()
        {
            _lock = true;
            if (DW4.AppConfig.LogDispatcher == false) return;
            DW4.Log($"{TAG} Lock");
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal void LockUpdate()
        {
            _lockUpdate = true;
            if (DW4.AppConfig.LogDispatcher == false) return;
            DW4.Log($"{TAG} Lock Update");
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal void Unlock()
        {
            _lock = false;
            if (DW4.AppConfig.LogDispatcher == false) return;
            DW4.Log($"{TAG} Unlock");
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal void UnlockUpdate()
        {
            _lockUpdate = false;
            if (DW4.AppConfig.LogDispatcher == false) return;
            DW4.Log($"{TAG} Unlock Update");
        }

        #endregion
    }
}