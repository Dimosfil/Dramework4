#if UNITY_EDITOR

using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using IG.HappyCoder.Plugins.Dramework4.Runtime.Tools;

using Sirenix.OdinInspector;

using Unity.IL2CPP.CompilerServices;

using UnityEditor;
using UnityEditor.SceneManagement;

using UnityEngine;
using UnityEngine.SceneManagement;


namespace IG.HappyCoder.Plugins.Dramework4.Runtime.Configs
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    internal partial class AppConfig
    {
        #region ================================ FIELDS

        private static AppConfig _instance;

        [FoldoutGroup("Project Settings")] [BoxGroup("Project Settings/Scenes", false)]
        [LabelWidth(LABEL_WIDTH_PROJECT_BASE)] [LabelText("Scenes:")]
        [SerializeField] [ValueDropdown("ScenePaths")]
        private string[] _scenes;

        #endregion

        #region ================================ PROPERTIES AND INDEXERS

        private static string TAG => $"[{nameof(AppConfig)}] :";
        internal IReadOnlyList<string> Scenes => _scenes;
        private IEnumerable ScenePaths
        {
            get
            {
                var scenePaths = new ValueDropdownList<string>();
                foreach (var sceneAsset in DW4.EditorTools.LoadAssets<SceneAsset>(string.Empty, new[] { _projectRootFolder }))
                {
                    var path = AssetDatabase.GetAssetPath(sceneAsset);
                    scenePaths.Add(path, path);
                }

                return scenePaths;
            }
        }

        #endregion

        #region ================================ METHODS

        [MenuItem("DW4/Scenes/Close Scene 1 %&1", false, 3)]
        public static void CloseScene1()
        {
            for (var i = 0; i < SceneManager.sceneCount; i++)
            {
                var scene = SceneManager.GetSceneAt(i);
                if (scene.path != _instance._scenes[0]) continue;
                EditorSceneManager.CloseScene(scene, true);
            }
        }

        [MenuItem("DW4/Scenes/Close Scene 2 %&2", false, 3)]
        public static void CloseScene2()
        {
            if (_instance._scenes.Length < 2)
            {
                Debug.LogError($"{TAG} Scenes count < 2");
                return;
            }

            for (var i = 0; i < SceneManager.sceneCount; i++)
            {
                var scene = SceneManager.GetSceneAt(i);
                if (scene.path != _instance._scenes[1]) continue;
                EditorSceneManager.CloseScene(scene, true);
            }
        }

        [MenuItem("DW4/Scenes/Close Scene 3 %&3", false, 3)]
        public static void CloseScene3()
        {
            if (_instance._scenes.Length < 3)
            {
                Debug.LogError($"{TAG} Scenes count < 3");
                return;
            }

            for (var i = 0; i < SceneManager.sceneCount; i++)
            {
                var scene = SceneManager.GetSceneAt(i);
                if (scene.path != _instance._scenes[2]) continue;
                EditorSceneManager.CloseScene(scene, true);
            }
        }

        [MenuItem("DW4/Scenes/Close Scene 4 %&4", false, 3)]
        public static void CloseScene4()
        {
            if (_instance._scenes.Length < 4)
            {
                Debug.LogError($"{TAG} Scenes count < 4");
                return;
            }

            for (var i = 0; i < SceneManager.sceneCount; i++)
            {
                var scene = SceneManager.GetSceneAt(i);
                if (scene.path != _instance._scenes[3]) continue;
                EditorSceneManager.CloseScene(scene, true);
            }
        }

        [MenuItem("DW4/Scenes/Close Scene 5 %&5", false, 3)]
        public static void CloseScene5()
        {
            if (_instance._scenes.Length < 5)
            {
                Debug.LogError($"{TAG} Scenes count < 5");
                return;
            }

            for (var i = 0; i < SceneManager.sceneCount; i++)
            {
                var scene = SceneManager.GetSceneAt(i);
                if (scene.path != _instance._scenes[4]) continue;
                EditorSceneManager.CloseScene(scene, true);
            }
        }

        [MenuItem("DW4/Scenes/Close Scene 6 %&6", false, 3)]
        public static void CloseScene6()
        {
            if (_instance._scenes.Length < 6)
            {
                Debug.LogError($"{TAG} Scenes count < 6");
                return;
            }

            for (var i = 0; i < SceneManager.sceneCount; i++)
            {
                var scene = SceneManager.GetSceneAt(i);
                if (scene.path != _instance._scenes[5]) continue;
                EditorSceneManager.CloseScene(scene, true);
            }
        }

        [MenuItem("DW4/Scenes/Open Scene 1 as Additive %#1", false, 3)]
        public static void OpenScene1AsAdditive()
        {
            EditorSceneManager.OpenScene(_instance._scenes[0], OpenSceneMode.Additive);
        }

        [MenuItem("DW4/Scenes/Open Scene 1 as Single %1", false, 3)]
        public static void OpenScene1AsSingle()
        {
            EditorSceneManager.OpenScene(_instance._scenes[0], OpenSceneMode.Single);
        }

        [MenuItem("DW4/Scenes/Open Scene 2 as Additive %#2", false, 3)]
        public static void OpenScene2AsAdditive()
        {
            if (_instance._scenes.Length < 2)
            {
                Debug.LogError($"{TAG} Scenes count < 2");
                return;
            }

            EditorSceneManager.OpenScene(_instance._scenes[1], OpenSceneMode.Additive);
        }

        [MenuItem("DW4/Scenes/Open Scene 2 as Single %2", false, 3)]
        public static void OpenScene2AsSingle()
        {
            if (_instance._scenes.Length < 2)
            {
                Debug.LogError($"{TAG} Scenes count < 2");
                return;
            }

            EditorSceneManager.OpenScene(_instance._scenes[1], OpenSceneMode.Single);
        }

        [MenuItem("DW4/Scenes/Open Scene 3 as Additive %#3", false, 3)]
        public static void OpenScene3AsAdditive()
        {
            if (_instance._scenes.Length < 3)
            {
                Debug.LogError($"{TAG} Scenes count < 3");
                return;
            }

            EditorSceneManager.OpenScene(_instance._scenes[2], OpenSceneMode.Additive);
        }

        [MenuItem("DW4/Scenes/Open Scene 3 as Single %3", false, 3)]
        public static void OpenScene3AsSingle()
        {
            if (_instance._scenes.Length < 3)
            {
                Debug.LogError($"{TAG} Scenes count < 3");
                return;
            }

            EditorSceneManager.OpenScene(_instance._scenes[2], OpenSceneMode.Single);
        }

        [MenuItem("DW4/Scenes/Open Scene 4 as Additive %#4", false, 3)]
        public static void OpenScene4AsAdditive()
        {
            if (_instance._scenes.Length < 4)
            {
                Debug.LogError($"{TAG} Scenes count < 4");
                return;
            }

            EditorSceneManager.OpenScene(_instance._scenes[3], OpenSceneMode.Additive);
        }

        [MenuItem("DW4/Scenes/Open Scene 4 as Single %4", false, 3)]
        public static void OpenScene4AsSingle()
        {
            if (_instance._scenes.Length < 4)
            {
                Debug.LogError($"{TAG} Scenes count < 4");
                return;
            }

            EditorSceneManager.OpenScene(_instance._scenes[3], OpenSceneMode.Single);
        }

        [MenuItem("DW4/Scenes/Open Scene 5 as Additive %#5", false, 3)]
        public static void OpenScene5AsAdditive()
        {
            if (_instance._scenes.Length < 5)
            {
                Debug.LogError($"{TAG} Scenes count < 5");
                return;
            }

            EditorSceneManager.OpenScene(_instance._scenes[4], OpenSceneMode.Additive);
        }

        [MenuItem("DW4/Scenes/Open Scene 5 as Single %5", false, 3)]
        public static void OpenScene5AsSingle()
        {
            if (_instance._scenes.Length < 5)
            {
                Debug.LogError($"{TAG} Scenes count < 5");
                return;
            }

            EditorSceneManager.OpenScene(_instance._scenes[4], OpenSceneMode.Single);
        }

        [MenuItem("DW4/Scenes/Open Scene 6 as Additive %#6", false, 3)]
        public static void OpenScene6AsAdditive()
        {
            if (_instance._scenes.Length < 6)
            {
                Debug.LogError($"{TAG} Scenes count < 6");
                return;
            }

            EditorSceneManager.OpenScene(_instance._scenes[5], OpenSceneMode.Additive);
        }

        [MenuItem("DW4/Scenes/Open Scene 6 as Single %6", false, 3)]
        public static void OpenScene6AsSingle()
        {
            if (_instance._scenes.Length < 6)
            {
                Debug.LogError($"{TAG} Scenes count < 6");
                return;
            }

            EditorSceneManager.OpenScene(_instance._scenes[5], OpenSceneMode.Single);
        }

        [MenuItem("DW4/Scenes/Add Open Scenes To Build Settings", false, 2)]
        private static void AddOpenScenesToBuildSettings()
        {
            var scenes = EditorBuildSettings.scenes.ToList();
            for (var i = 0; i < SceneManager.sceneCount; i++)
            {
                var path = SceneManager.GetSceneAt(i).path;
                if (scenes.Any(s => s.path == path)) return;
                scenes.Add(new EditorBuildSettingsScene(path, true));
            }

            EditorBuildSettings.scenes = scenes.ToArray();
        }

        [MenuItem("DW4/Scenes/Add Selected Scene To Build Settings", false, 2)]
        private static void AddSelectedSceneToBuildSettings()
        {
            if (Selection.activeObject == null || Selection.activeObject is SceneAsset == false) return;
            var path = AssetDatabase.GetAssetPath(Selection.activeObject);
            var scenes = EditorBuildSettings.scenes.ToList();
            if (scenes.Any(s => s.path == path)) return;
            scenes.Add(new EditorBuildSettingsScene(path, true));
            EditorBuildSettings.scenes = scenes.ToArray();
        }

        [MenuItem("DW4/Scenes/Cleanup Build Settings Scenes List", false, 1)]
        private static void CleanupBuildSettingsSceneList()
        {
            EditorBuildSettings.scenes = EditorBuildSettings.scenes.Where(s => File.Exists(s.path)).ToArray();
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        [MenuItem("DW4/Configs/Select App Config %`")]
        private static void SelectConfig()
        {
            var config = DW4.EditorTools.LoadAsset<AppConfig>(FILENAME);
            Selection.activeObject = config;
        }

        private void OnValidate()
        {
            _instance = this;
            name = FILENAME;
        }

        #endregion
    }
}

#endif