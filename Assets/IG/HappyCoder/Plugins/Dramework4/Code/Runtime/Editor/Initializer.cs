#if UNITY_EDITOR

using System;
using System.IO;
using System.Reflection;

using IG.HappyCoder.Plugins.Dramework4.Runtime.Attributes.Initialization;
using IG.HappyCoder.Plugins.Dramework4.Runtime.Configs;
using IG.HappyCoder.Plugins.Dramework4.Runtime.Constants;
using IG.HappyCoder.Plugins.Dramework4.Runtime.Core;

using UnityEditor;

using UnityEngine;


namespace IG.HappyCoder.Plugins.Dramework4.Runtime.Editor
{
    internal static class Initializer
    {
        #region ================================ METHODS

        private static void CreateConfigs()
        {
            var path = Path.Combine(Paths.RESOURCES_PATH, $"{AppConfig.FILENAME}.asset");
            if (File.Exists(path)) return;
            var appConfig = ScriptableObject.CreateInstance<AppConfig>();
            AssetDatabase.CreateAsset(appConfig, path);
        }

        [InitializeOnLoadMethod]
        private static void Initialize()
        {
            InitializeDirectories();
            CreateConfigs();

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            EditorApplication.playModeStateChanged -= ResetOnPlayModeStateChanged;
            EditorApplication.playModeStateChanged += ResetOnPlayModeStateChanged;
        }

        private static void InitializeDirectories()
        {
            if (AssetDatabase.IsValidFolder(Paths.PLUGINS_PATH) == false)
                AssetDatabase.CreateFolder("Assets", Path.GetFileNameWithoutExtension(Paths.PLUGINS_PATH));

            if (AssetDatabase.IsValidFolder(Paths.DRAMEWORK4_PATH) == false)
                AssetDatabase.CreateFolder(Paths.PLUGINS_PATH, Path.GetFileNameWithoutExtension(Paths.DRAMEWORK4_PATH));

            if (AssetDatabase.IsValidFolder(Paths.RESOURCES_PATH) == false)
                AssetDatabase.CreateFolder(Paths.DRAMEWORK4_PATH, Path.GetFileNameWithoutExtension(Paths.RESOURCES_PATH));
        }

        private static void ResetOnPlayModeStateChanged(PlayModeStateChange state)
        {
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                if (Dispatcher.IsSystemAssembly(assembly)
                    || Dispatcher.IsUnityAssembly(assembly)
                    || Dispatcher.IsPluginAssembly(assembly)) continue;

                var types = assembly.GetTypes();
                for (var i = 0; i < types.Length; i++)
                {
                    var type = types[i];

                    foreach (var fieldInfo in type.GetFields(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic))
                    {
                        var resetAttribute = fieldInfo.GetCustomAttribute<SetOnPlayModeStateChangedAttribute>();
                        if (resetAttribute == null) continue;
                        fieldInfo.SetValue(null, resetAttribute.Value);
                    }

                    foreach (var propertyInfo in type.GetProperties(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic))
                    {
                        var resetAttribute = propertyInfo.GetCustomAttribute<SetOnPlayModeStateChangedAttribute>();
                        if (resetAttribute == null) continue;
                        propertyInfo.SetValue(null, resetAttribute.Value);
                    }
                }
            }
        }

        #endregion
    }
}

#endif