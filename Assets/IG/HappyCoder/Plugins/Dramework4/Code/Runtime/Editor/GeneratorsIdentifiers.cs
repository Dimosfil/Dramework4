#if UNITY_EDITOR

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

using IG.HappyCoder.Plugins.Dramework4.Runtime.Core;
using IG.HappyCoder.Plugins.Dramework4.Runtime.Interfaces;
using IG.HappyCoder.Plugins.Dramework4.Runtime.Tools;

using Newtonsoft.Json;

using UnityEditor;
using UnityEditor.AddressableAssets;

using UnityEditorInternal;

using UnityEngine;
using UnityEngine.U2D;

using AnimatorController = UnityEditor.Animations.AnimatorController;
using AnimatorControllerLayer = UnityEditor.Animations.AnimatorControllerLayer;
using AnimatorControllerParameter = UnityEngine.AnimatorControllerParameter;
using Object = UnityEngine.Object;


namespace IG.HappyCoder.Plugins.Dramework4.Runtime.Editor
{
    internal static partial class Generators
    {
        #region ================================ FIELDS

        private const string NAMESPACE = "namespace IG.HappyCoder.Dramework4.Runtime.Generated";
        private const string PATH_INSTANCES = "Assets/Plugins/Dramework4/Runtime/Generated/Instances";
        private const string PATH_ADDRESSABLES = "Assets/Plugins/Dramework4/Runtime/Generated/Addressables";
        private const string PATH_ANIMATORS = "Assets/Plugins/Dramework4/Runtime/Generated/Animators";
        private const string PATH_ATLASES = "Assets/Plugins/Dramework4/Runtime/Generated/Atlases";

        private const string BUILT_IN_DATA_NAME = "Built In Data";

        #endregion

        #region ================================ PROPERTIES AND INDEXERS

        private static string ProjectRootFolder => DW4.AppConfig.ProjectRootFolder;

        #endregion

        #region ================================ METHODS

        private static void DeleteDirectory(string path)
        {
            if (Directory.Exists(path) == false) return;
            AssetDatabase.DeleteAsset(DW4.IOTools.GetRelativePath(path));
        }

        private static void GenerateAddressableLabelsFile()
        {
            if (AddressableAssetSettingsDefaultObject.Settings == null) return;

            const string className = "AdrLabel";
            var labels = AddressableAssetSettingsDefaultObject.Settings.GetLabels().Where(l => l != "default").ToArray();
            var file = GetBaseFile(NAMESPACE, className, labels);
            var path = Path.Combine(PATH_ADDRESSABLES, $"{className}.cs");

            if (labels.Length == 0)
            {
                if (File.Exists(path))
                {
                    File.Delete(path);
                }
                return;
            }

            if (File.Exists(path) && File.ReadAllText(path) == file) return;
            CreateDirectory(PATH_ADDRESSABLES);
            File.WriteAllText(path, file);
        }

        private static void GenerateAddressableNamesFile()
        {
            if (AddressableAssetSettingsDefaultObject.Settings == null) return;

            foreach (var addressableAssetGroup in AddressableAssetSettingsDefaultObject.Settings.groups)
            {
                if (addressableAssetGroup.Name == "Built In Data") continue;

                var names = addressableAssetGroup.entries
                    .Select(e => e.address)
                    .OrderBy(a => a)
                    .ToArray();

                if (names.Length == 0) continue;

                var className = $"{DW4.StringTools.ClearText(addressableAssetGroup.Name)}Name";
                var addressableNames = new List<string>();
                foreach (var addressableName in names)
                {
                    if (addressableNames.Contains(addressableName)) continue;
                    addressableNames.Add(addressableName);
                }
                var file = GetBaseFile(NAMESPACE, className, addressableNames);
                var path = Path.Combine(PATH_ADDRESSABLES, $"{className}.cs");
                if (File.Exists(path) && File.ReadAllText(path) == file) continue;
                CreateDirectory(PATH_ADDRESSABLES);
                File.WriteAllText(path, file);
            }
        }

        private static void GenerateAnimatorsInfoFile()
        {
            var guids = AssetDatabase.FindAssets("t:AnimatorController", new[] { ProjectRootFolder });
            var animatorControllers = guids.Select(g => AssetDatabase.LoadAssetAtPath<AnimatorController>(AssetDatabase.GUIDToAssetPath(g))).ToArray();

            if (animatorControllers.Length == 0)
            {
                DeleteDirectory(PATH_ANIMATORS);
                return;
            }

            var paths = new List<string>();
            foreach (var animatorController in animatorControllers)
            {
                var className = $"Animator{DW4.StringTools.ClearText(animatorController.name)}";
                var layers = new HashSet<AnimatorControllerLayer>(animatorController.layers).ToArray();
                var clips = new HashSet<AnimationClip>(animatorController.animationClips.OrderBy(c => c.name));
                var file = GetAnimatorInfoFile(NAMESPACE, className, layers, clips, animatorController.parameters);
                var path = Path.Combine(PATH_ANIMATORS, $"{className}.cs");
                paths.Add(path);
                if (File.Exists(path) && File.ReadAllText(path) == file) continue;
                CreateDirectory(PATH_ANIMATORS);
                File.WriteAllText(path, file);
            }

            var key = $"{Application.productName}_Dramework4_Animators_Info";
            var json = PlayerPrefs.GetString(key);
            if (string.IsNullOrEmpty(json) == false)
            {
                var lastPaths = JsonConvert.DeserializeObject<List<string>>(json);
                foreach (var path in lastPaths.Where(path => paths.Contains(path) == false && File.Exists(path)))
                    File.Delete(path);
            }

            json = JsonConvert.SerializeObject(paths);
            PlayerPrefs.SetString(key, json);
        }

        private static void GenerateAudioClipNamesFile()
        {
            var audioClipNames = AssetDatabase.FindAssets("t:AudioClip", new[] { ProjectRootFolder })
                .Select(AssetDatabase.GUIDToAssetPath)
                .Select(AssetDatabase.LoadAssetAtPath<AudioClip>)
                .Select(clip => clip.name)
                .ToList();

            if (audioClipNames.Count == 0) return;

            const string className = "AudioClipName";
            var file = GetAudioClipsFile(NAMESPACE, className, audioClipNames);
            var path = Path.Combine(PATH_GENERATED_FOLDER, $"{className}.cs");
            if (File.Exists(path) && File.ReadAllText(path) == file) return;
            CreateDirectory(PATH_GENERATED_FOLDER);
            File.WriteAllText(path, file);
        }

        private static void GenerateIdentifiers()
        {
            GenerateSceneNamesFile();
            GenerateSceneInstancesFile();
            GenerateAddressableNamesFile();
            GenerateAddressableLabelsFile();
            GeneratePrefabNamesFile();
            GenerateAudioClipNamesFile();
            GenerateResourceNamesFile();
            GenerateAnimatorsInfoFile();
            GenerateSpriteAtlasKeysFile();
            GenerateTagsFile();
        }

        private static void GeneratePrefabNamesFile()
        {
            var prefabNames = AssetDatabase.FindAssets("t:prefab", new[] { ProjectRootFolder })
                .Select(AssetDatabase.GUIDToAssetPath)
                .Select(AssetDatabase.LoadAssetAtPath<GameObject>)
                .Select(o => DW4.StringTools.ClearText(o.name))
                .ToHashSet();

            if (prefabNames.Count == 0) return;

            const string className = "PrefabName";
            var file = GetBaseFile(NAMESPACE, className, prefabNames);
            var path = Path.Combine(PATH_GENERATED_FOLDER, $"{className}.cs");
            if (File.Exists(path) && File.ReadAllText(path) == file) return;
            CreateDirectory(PATH_GENERATED_FOLDER);
            File.WriteAllText(path, file);
        }

        private static void GenerateResourceNamesFile()
        {
            var assetPaths = AssetDatabase.GetAllAssetPaths();
            var resourcePaths = new HashSet<string>();

            foreach (var assetPath in assetPaths)
            {
                var extension = Path.GetExtension(assetPath);
                if (string.IsNullOrEmpty(extension)
                    || assetPath.Contains(ProjectRootFolder) == false
                    || (assetPath.Contains("/Resources/") == false
                        && assetPath.EndsWith("/Resources") == false)) continue;

                var resourcePath = assetPath.Remove(0, assetPath.IndexOf("Resources/", StringComparison.Ordinal) + 10);
                resourcePath = resourcePath.Replace(extension, string.Empty);
                resourcePaths.Add(resourcePath);
            }

            if (resourcePaths.Count == 0) return;

            const string className = "ResourceName";
            var file = GetBaseFile(NAMESPACE, className, resourcePaths);
            var path = Path.Combine(PATH_GENERATED_FOLDER, $"{className}.cs");
            if (File.Exists(path) && File.ReadAllText(path) == file) return;
            CreateDirectory(PATH_GENERATED_FOLDER);
            File.WriteAllText(path, file);
        }

        private static void GenerateSceneInstancesFile()
        {
            var sceneContainer = Object.FindObjectOfType<SceneContainer>(true);

            if (sceneContainer == null) return;

            sceneContainer.Refresh();
            var sceneName = sceneContainer.gameObject.scene.name;
            var className = $"{sceneName}ID";
            var instanceIDs = new List<string>();
            foreach (var objectToBind in sceneContainer.ObjectsToBind)
            {
                if (objectToBind is IIdentifiable identifiable == false || string.IsNullOrEmpty(identifiable.ID)) continue;

                if (instanceIDs.Contains(identifiable.ID))
                {
                    DW4.LogError($"Found double instance ID: \"{identifiable.ID}\" on scene \"{sceneName}\"");
                    continue;
                }
                instanceIDs.Add(identifiable.ID);
            }

            var file = GetBaseFile(NAMESPACE, className, instanceIDs);
            var path = Path.Combine(PATH_INSTANCES, $"{className}.cs");

            if (instanceIDs.Count == 0)
            {
                if (File.Exists(path))
                    File.Delete(path);

                return;
            }

            if (File.Exists(path) && File.ReadAllText(path) == file) return;
            CreateDirectory(PATH_INSTANCES);
            File.WriteAllText(path, file);

            if (Directory.Exists(PATH_INSTANCES) == false) return;
            var files = Directory.GetFiles(PATH_INSTANCES);
            if (files.Length > 0) return;
            AssetDatabase.DeleteAsset(PATH_INSTANCES);
            AssetDatabase.Refresh();
        }

        private static void GenerateSceneNamesFile()
        {
            var scenes = DW4.EditorTools.LoadAssets<SceneAsset>(string.Empty, new[] { ProjectRootFolder })
                .OrderBy(s => s.name)
                .ToArray();

            if (scenes.Length == 0) return;

            const string className = "SceneName";
            var sceneNames = new List<string> { "DontDestroyOnLoad" };
            foreach (var sceneName in scenes.Select(s => s.name))
            {
                if (sceneNames.Contains(sceneName)) continue;
                sceneNames.Add(sceneName);
            }
            var file = GetBaseFile(NAMESPACE, className, sceneNames);
            var path = Path.Combine(PATH_GENERATED_FOLDER, $"{className}.cs");
            if (File.Exists(path) && File.ReadAllText(path) == file) return;
            CreateDirectory(PATH_GENERATED_FOLDER);
            File.WriteAllText(path, file);
        }

        private static void GenerateSpriteAtlasKeysFile()
        {
            var spriteAtlases = DW4.EditorTools.LoadAssets<SpriteAtlas>(string.Empty, new[] { ProjectRootFolder }).ToArray();

            if (spriteAtlases.Length == 0)
            {
                DeleteDirectory(PATH_ATLASES);
                return;
            }

            var paths = new List<string>();
            foreach (var spriteAtlas in spriteAtlases)
            {
                var sprites = new Sprite[spriteAtlas.spriteCount];
                spriteAtlas.GetSprites(sprites);
                sprites = sprites.OrderBy(s => s.name).ToArray();
                var className = $"{DW4.StringTools.ClearText(spriteAtlas.name)}";
                var file = GetBaseFile(NAMESPACE, className, sprites.Select(s => s.name.Replace("(Clone)", "")));
                var path = Path.Combine(PATH_ATLASES, $"{className}.cs");
                paths.Add(path);
                if (File.Exists(path) && File.ReadAllText(path) == file) continue;
                CreateDirectory(PATH_ATLASES);
                File.WriteAllText(path, file);
            }

            var key = $"{Application.productName}_Dramework4_Sprite_Atlas_Keys";
            var json = PlayerPrefs.GetString(key);
            if (string.IsNullOrEmpty(json) == false)
            {
                var lastPaths = JsonConvert.DeserializeObject<List<string>>(json);
                foreach (var path in lastPaths.Where(path => paths.Contains(path) == false && File.Exists(path)))
                    File.Delete(path);
            }

            json = JsonConvert.SerializeObject(paths);
            PlayerPrefs.SetString(key, json);
        }

        private static void GenerateTagsFile()
        {
            const string className = "Tags";
            var file = GetBaseFile(NAMESPACE, className, InternalEditorUtility.tags);
            var path = Path.Combine(PATH_GENERATED_FOLDER, $"{className}.cs");
            if (File.Exists(path) && File.ReadAllText(path) == file) return;
            CreateDirectory(PATH_GENERATED_FOLDER);
            File.WriteAllText(path, file);
        }

        private static string GetAnimatorInfoFile(string nameSpace,
            string className,
            IReadOnlyList<AnimatorControllerLayer> layers,
            IEnumerable<AnimationClip> clips,
            IEnumerable<AnimatorControllerParameter> parameters)
        {
            var index = 0;
            var layersData = layers.ToDictionary(layer => layer.name, _ => index++);
            var stateNameHashes = new Dictionary<string, int>();
            // var clipNameHashes = new Dictionary<string, int>();
            var clipsNameLength = new HashSet<(int layerIndex, string stateName, float length)>();
            var clipsHashLength = new HashSet<(int layerIndex, int stateHash, float length)>();

            for (var i = 0; i < layers.Count; i++)
            {
                var layer = layers[i];
                foreach (var state in layer.stateMachine.states)
                {
                    stateNameHashes.TryAdd(state.state.name, state.state.nameHash);
                    if (state.state.motion is AnimationClip clip)
                    {
                        // clipNameHashes.TryAdd(state.state.name, Animator.StringToHash(clip.name));
                        clipsNameLength.Add((i, state.state.name, clip.length));
                        clipsHashLength.Add((i, state.state.nameHash, clip.length));
                    }
                }
            }

            var file = "using System.Collections.Generic;\n\n";
            file += "using System.Linq;\n\n";
            file += nameSpace;
            file += "\n{";

            file += $"\n\tpublic static class {className}";
            file += "\n\t{";

            file += "\n\n\t\tpublic static class Layer";
            file += "\n\t\t{";
            file = layersData.Aggregate(file, (current, pair) => current + $"\n\t\t\tpublic const int {DW4.StringTools.ClearText(pair.Key)} = {pair.Value};");
            file += "\n\t\t}";

            file += "\n\n\t\tpublic static class State";
            file += "\n\t\t{";
            file = stateNameHashes.Aggregate(file, (current, pair) => current + $"\n\t\t\tpublic const int {DW4.StringTools.ClearText(pair.Key)} = {pair.Value};");
            file += "\n\t\t}";

            file += "\n\n\t\tpublic enum States";
            file += "\n\t\t{";
            file = stateNameHashes.Aggregate(file, (current, state) => current + $"\n\t\t\t{DW4.StringTools.ClearText(state.Key)} = {state.Value},");
            file += "\n\t\t}";

            // file += "\n\n\t\tpublic static class ClipNameHashes";
            // file += "\n\t\t{";
            // file = clipNameHashes.Aggregate(file, (current, pair) => current + $"\n\t\t\tpublic const int {DW4.StringTools.ClearText(pair.Key)} = {pair.Value};");
            // file += "\n\t\t}";
            //
            // file += "\n\n\t\tpublic enum ClipNameHash";
            // file += "\n\t\t{";
            // file = clipNameHashes.Aggregate(file, (current, pair) => current + $"\n\t\t\t{DW4.StringTools.ClearText(pair.Key)} = {pair.Value},");
            // file += "\n\t\t}";

            file += "\n\n\t\tpublic static class Parameters";
            file += "\n\t\t{";
            file = parameters.Aggregate(file, (current, parameter) => current + $"\n\t\t\tpublic const int {DW4.StringTools.ClearText(parameter.name)} = {parameter.nameHash};");
            file += "\n\t\t}";

            file += "\n\n\t\t public static float GetClipLength(int layerIndex, string stateName)";
            file += "\n\t\t{";
            file += "\n\t\t\treturn ClipLengthByName.FirstOrDefault(i => i.layerIndex == layerIndex && i.stateName == stateName).length;";
            file += "\n\t\t}";

            file += "\n\n\t\t public static float GetClipLength(int layerIndex, int stateNameHash)";
            file += "\n\t\t{";
            file += "\n\t\t\treturn ClipLengthByNameHash.FirstOrDefault(i => i.layerIndex == layerIndex && i.stateNameHash == stateNameHash).length;";
            file += "\n\t\t}";

            file += "\n\n\t\t private static readonly IReadOnlyList<(int layerIndex, string stateName, float length)> ClipLengthByName = new List<(int, string, float)>";
            file += "\n\t\t{";
            file = clipsNameLength.Aggregate(file, (current, item) => current + $"\n\t\t\t({item.layerIndex}, \"{item.stateName}\", {item.length.ToString(CultureInfo.InvariantCulture)}f),");
            file += "\n\t\t};";

            file += "\n\n\t\t private static readonly IReadOnlyList<(int layerIndex, int stateNameHash, float length)> ClipLengthByNameHash = new List<(int, int, float)>";
            file += "\n\t\t{";
            file = clipsHashLength.Aggregate(file, (current, item) => current + $"\n\t\t\t({item.layerIndex}, {item.stateHash}, {item.length.ToString(CultureInfo.InvariantCulture)}f),");
            file += "\n\t\t};";

            file += "\n\t}";

            file += "\n}";
            return file;
        }

        private static string GetAudioClipsFile(string nameSpace, string className, IReadOnlyList<string> fields)
        {
            var file = "using System.Collections.Generic;\n\n";
            file += nameSpace;
            file += "\n{";
            file += $"\n\tpublic static class {className}";
            file += "\n\t{";
            file = fields.Aggregate(file, (current, field) => current + $"\n\t\tpublic const string {DW4.StringTools.ClearText(field)} = \"{field}\";");
            file += "\n\n\t\t public static readonly List<string> ClipNames = new List<string>";
            file += "\n\t\t{";
            file = fields.Aggregate(file, (current, field) => current + $"\n\t\t\t\"{field}\",");
            file += "\n\t\t};";
            file += "\n\t}";
            file += "\n}";
            return file;
        }

        private static string GetBaseFile(string nameSpace, string className, IEnumerable<string> fields)
        {
            var file = nameSpace;
            file += "\n{";
            file += $"\n\tpublic static class {className}";
            file += "\n\t{";
            file = fields.Aggregate(file, (current, field) => current + $"\n\t\tpublic const string {DW4.StringTools.ClearText(field)} = \"{field}\";");
            file += "\n\t}";
            file += "\n}";
            return file;
        }

        [MenuItem("DW4/Generators/Identifiers")]
        private static void MenuGenerateIdentifiers()
        {
            GenerateIdentifiers();

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        #endregion
    }
}

#endif