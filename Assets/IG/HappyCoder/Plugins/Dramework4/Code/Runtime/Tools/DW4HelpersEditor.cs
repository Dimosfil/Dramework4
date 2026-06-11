#if UNITY_EDITOR

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

using Unity.IL2CPP.CompilerServices;

using UnityEditor;
using UnityEditor.AddressableAssets;

using UnityEngine;

using Object = UnityEngine.Object;


namespace IG.HappyCoder.Plugins.Dramework4.Runtime.Tools
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    [SuppressMessage("ReSharper", "UnusedType.Global")]
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    public static partial class DW4
    {
        #region ================================ PROPERTIES AND INDEXERS

        public static bool IsDomainReloadDisabled => EditorSettings.enterPlayModeOptionsEnabled &&
                                                     EditorSettings.enterPlayModeOptions.HasFlag(EnterPlayModeOptions.DisableDomainReload);

        #endregion

        #region ================================ NESTED TYPES

        [Il2CppSetOption(Option.NullChecks, false)]
        [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
        [Il2CppSetOption(Option.DivideByZeroChecks, false)]
        public static partial class AddressablesTools
        {
            #region ================================ PROPERTIES AND INDEXERS

            public static IEnumerable<string> GroupNames => AddressableAssetSettingsDefaultObject.Settings.groups.Select(g => g.Name);

            #endregion
        }

        [Il2CppSetOption(Option.NullChecks, false)]
        [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
        [Il2CppSetOption(Option.DivideByZeroChecks, false)]
        public static class EditorTools
        {
            #region ================================ METHODS

            public static string FindAsset<T>(string assetName = "", string[] defaultPath = null, bool skipLog = false) where T : Object
            {
                var filter = string.IsNullOrEmpty(assetName) ? $"t:{typeof(T).Name}" : $"t:{typeof(T).Name} {assetName}";
                defaultPath ??= new[] { "Assets" };
                var guids = AssetDatabase.FindAssets(filter, defaultPath);
                if (guids.Length == 0)
                {
                    if (skipLog == false)
                        LogError(string.IsNullOrEmpty(assetName) ? $"No one asset type of \"{typeof(T).Name}\" was found" : $"No one asset type of \"{typeof(T).Name}\" and by name \"{assetName}\" was found");
                    return default;
                }

                if (guids.Length > 1)
                {
                    if (skipLog == false)
                        LogError(string.IsNullOrEmpty(assetName) ? $"More than one asset type of \"{typeof(T).Name}\" was found" : $"More than one asset \"{assetName}\" type of \"{typeof(T).Name}\" was found");
                    return default;
                }

                return guids[0];
            }

            public static IReadOnlyList<string> FindAssetGuids<T>(string assetName = "", string[] defaultPaths = null) where T : Object
            {
                var filter = string.IsNullOrEmpty(assetName) ? $"t:{typeof(T).Name}" : $"t:{typeof(T).Name} {assetName}";
                defaultPaths ??= new[] { "Assets" };
                var guids = AssetDatabase.FindAssets(filter, defaultPaths);
                if (guids.Length > 0) return guids;
                return Array.Empty<string>();
            }

            public static T LoadAsset<T>(string assetName, string[] defaultPath = null, bool skipLog = false) where T : Object
            {
                var guid = FindAsset<T>(assetName, defaultPath, skipLog);
                var path = AssetDatabase.GUIDToAssetPath(guid);
                return AssetDatabase.LoadAssetAtPath<T>(path);
            }

            public static IReadOnlyList<T> LoadAssets<T>(string assetName, string[] defaultPaths = null) where T : Object
            {
                var assets = new List<T>();
                var guids = FindAssetGuids<T>(assetName, defaultPaths);
                if (guids.Any() == false) return assets;
                var paths = guids.Select(AssetDatabase.GUIDToAssetPath).ToArray();
                foreach (var path in paths)
                {
                    var asset = AssetDatabase.LoadAssetAtPath<T>(path);
                    if (asset == null)
                    {
                        Debug.LogError($"Asset at path «{path}» is null");
                        continue;
                    }
                    assets.Add(asset);
                }
                return assets;
            }

            #endregion
        }

        #endregion
    }
}

#endif