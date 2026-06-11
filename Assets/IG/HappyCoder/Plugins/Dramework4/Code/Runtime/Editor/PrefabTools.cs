#if UNITY_EDITOR

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Newtonsoft.Json;

using UnityEditor;
using UnityEditor.SceneManagement;

using UnityEngine;

using Object = UnityEngine.Object;


namespace IG.HappyCoder.Plugins.Dramework4.Runtime.Editor
{
    [InitializeOnLoad]
    internal static class PrefabTools
    {
        #region ================================ FIELDS

        private static readonly string ExportPathKey = "PrefabTools.ExportPath";
        private static readonly string SavePath = $"{Application.dataPath}/../Temp/ig.prefab.utils.dat";

        #endregion

        #region ================================ CONSTRUCTORS AND DESTRUCTOR

        static PrefabTools()
        {
            EditorApplication.wantsToQuit += OnEditorApplicationWantsToQuit;
            EditorApplication.hierarchyWindowItemOnGUI += HierarchyWindowItemOnGui;
        }

        #endregion

        #region ================================ METHODS

        [MenuItem("GameObject/Happy Coder/Editor Tools/Prefabs/Export Prefab", false, 23)]
        private static void ExportPackage()
        {
            var activeGameObject = Selection.activeGameObject;
            if (activeGameObject == null || activeGameObject.activeInHierarchy == false)
            {
                Debug.LogError("No one GameObject is selected in hierarchy");
                return;
            }

            var prefab = PrefabUtility.GetCorrespondingObjectFromSource(activeGameObject);
            if (prefab == null)
            {
                Debug.LogError($"Prefab for \"{activeGameObject.name}\" is not found in \"Assets\"");
                return;
            }

            var prefabPath = AssetDatabase.GetAssetPath(prefab);
            var exportPath = PlayerPrefs.GetString(ExportPathKey);
            if (string.IsNullOrEmpty(exportPath) || Directory.Exists(Path.GetDirectoryName(exportPath)) == false)
            {
                exportPath = EditorUtility.SaveFilePanel("Export to", "Assets", $"{prefab.name}", "unitypackage");
                if (string.IsNullOrEmpty(exportPath)) return;
                PlayerPrefs.SetString(ExportPathKey, exportPath);
                PlayerPrefs.Save();
            }
            AssetDatabase.ExportPackage(prefabPath, exportPath, ExportPackageOptions.IncludeDependencies);
            AssetDatabase.Refresh();
        }

        [MenuItem("GameObject/Happy Coder/Editor Tools/Prefabs/Export Prefab As", false, 24)]
        private static void ExportPackageAs()
        {
            var activeGameObject = Selection.activeGameObject;
            if (activeGameObject == null || activeGameObject.activeInHierarchy == false)
            {
                Debug.LogError("No one GameObject is selected in hierarchy");
                return;
            }

            var prefab = PrefabUtility.GetCorrespondingObjectFromSource(activeGameObject);
            if (prefab == null)
            {
                Debug.LogError($"Prefab for \"{activeGameObject.name}\" is not found in \"Assets\"");
                return;
            }

            var prefabPath = AssetDatabase.GetAssetPath(prefab);
            var exportPath = EditorUtility.SaveFilePanel("Export to", "Assets", $"{prefab.name}", "unitypackage");
            if (string.IsNullOrEmpty(exportPath)) return;
            PlayerPrefs.SetString(ExportPathKey, exportPath);
            PlayerPrefs.Save();
            AssetDatabase.ExportPackage(prefabPath, exportPath, ExportPackageOptions.IncludeDependencies);
            AssetDatabase.Refresh();
        }

        private static UnpackDataContainer GetContainer()
        {
            var json = File.Exists(SavePath) ? File.ReadAllText(SavePath) : "";
            return string.IsNullOrEmpty(json) ? new UnpackDataContainer() : JsonUtility.FromJson<UnpackDataContainer>(json);
        }

        private static string GetGlobalObjectId(Object obj)
        {
            return GlobalObjectId.GetGlobalObjectIdSlow(obj).ToString();
        }

        private static void HierarchyWindowItemOnGui(int instanceId, Rect selectionRect)
        {
            var selected = Selection.activeGameObject;
            if (selected == null || selected.activeInHierarchy == false) return;
            var e = Event.current;
            if (e.control == false || e.alt == false || e.button != 0 || e.isMouse == false) return;
            var container = GetContainer();
            var root = selected.transform.root.gameObject;
            if (container.Contains(GetGlobalObjectId(root), GetGlobalObjectId(selected)))
            {
                Pack();
                Selection.activeGameObject = null;
            }
            else
            {
                if (Unpack())
                    Selection.activeGameObject = null;
            }
        }

        private static bool OnEditorApplicationWantsToQuit()
        {
            var container = GetContainer();
            foreach (var item in container.Items)
            {
                for (var i = item.Data.Count - 1; i >= 0; i--)
                {
                    GlobalObjectId.TryParse(item.Data[i].GlobalObjectId, out var globalObjectId);
                    var obj = (GameObject)GlobalObjectId.GlobalObjectIdentifierToObjectSlow(globalObjectId);
                    if (obj == null) continue;
                    PrefabUtility.SaveAsPrefabAssetAndConnect(obj, item.Data[i].Path, InteractionMode.AutomatedAction);
                    EditorSceneManager.SaveScene(obj.scene);
                }
            }
            return true;
        }

        [MenuItem("GameObject/Happy Coder/Editor Tools/Prefabs/Save Prefab", false, 22)]
        private static void Pack()
        {
            if (EditorApplication.isPlayingOrWillChangePlaymode) return;
            if (Selection.activeGameObject != null && Selection.activeGameObject.transform.root.gameObject == Selection.activeGameObject)
                SaveAllPrefabs();
            else
                SavePrefab();
        }

        private static void SaveAllPrefabs()
        {
            var key = GetGlobalObjectId(Selection.activeGameObject);
            var container = GetContainer();

            foreach (var item in container.Items)
            {
                if (item.Key != key) continue;
                for (var i = item.Data.Count - 1; i >= 0; i--)
                {
                    GlobalObjectId.TryParse(item.Data[i].GlobalObjectId, out var globalObjectId);
                    var obj = (GameObject)GlobalObjectId.GlobalObjectIdentifierToObjectSlow(globalObjectId);
                    if (obj == null) continue;
                    PrefabUtility.SaveAsPrefabAssetAndConnect(obj, item.Data[i].Path, InteractionMode.AutomatedAction);
                }
            }

            container.Clear(key);
            SaveContainer(container);
        }

        private static void SaveContainer(UnpackDataContainer container)
        {
            File.WriteAllText(SavePath, JsonConvert.SerializeObject(container));
        }

        private static void SavePrefab()
        {
            var selected = Selection.activeGameObject;
            if (selected == null || string.IsNullOrEmpty(selected.scene.name)) return;
            var container = GetContainer();
            var root = selected.transform.root.gameObject;
            var data = container.GetData(GetGlobalObjectId(root), GetGlobalObjectId(selected));
            if (data == null) return;
            PrefabUtility.SaveAsPrefabAssetAndConnect(selected, data.Path, InteractionMode.AutomatedAction);
            container.RemoveData(GetGlobalObjectId(root), GetGlobalObjectId(selected));
            SaveContainer(container);
        }

        [MenuItem("GameObject/Happy Coder/Editor Tools/Prefabs/Unpack Prefab", false, 21)]
        // ReSharper disable once Unity.IncorrectMethodSignature
        private static bool Unpack()
        {
            if (EditorApplication.isPlayingOrWillChangePlaymode) return false;
            var selected = Selection.activeGameObject;
            if (selected == null || string.IsNullOrEmpty(selected.scene.name)) return false;
            if (PrefabUtility.GetPrefabInstanceStatus(selected) != PrefabInstanceStatus.Connected || PrefabUtility.IsOutermostPrefabInstanceRoot(selected) == false) return false;

            var container = GetContainer();
            var path = PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(selected);
            var root = selected.transform.root.gameObject;
            PrefabUtility.UnpackPrefabInstance(selected, PrefabUnpackMode.OutermostRoot, InteractionMode.AutomatedAction);
            container.AddItem(GetGlobalObjectId(root), GetGlobalObjectId(selected), path);
            SaveContainer(container);
            return true;
        }

        #endregion

        #region ================================ NESTED TYPES

        [Serializable]
        private class UnpackData
        {
            #region ================================ FIELDS

            public string GlobalObjectId;
            public string Path;

            #endregion

            #region ================================ CONSTRUCTORS AND DESTRUCTOR

            public UnpackData(string globalObjectId, string path)
            {
                GlobalObjectId = globalObjectId;
                Path = path;
            }

            #endregion
        }

        [Serializable]
        private class UnpackDataContainer
        {
            #region ================================ FIELDS

            public List<UnpackDataItem> Items = new List<UnpackDataItem>();

            #endregion

            #region ================================ PROPERTIES AND INDEXERS

            public List<UnpackData> this[int index] => Items[index]?.Data;

            #endregion

            #region ================================ METHODS

            public void AddItem(string key, string globalObjectId, string path)
            {
                var item = Items.FirstOrDefault(i => i.Key == key);
                if (item != null)
                {
                    var data = item.Data.FirstOrDefault(d => d.GlobalObjectId == globalObjectId);
                    if (data != null)
                    {
                        data.Path = path;
                    }
                    else
                    {
                        item.Data.Add(new UnpackData(globalObjectId, path));
                    }
                }
                else
                {
                    item = new UnpackDataItem { Key = key };
                    item.Data.Add(new UnpackData(globalObjectId, path));
                    Items.Add(item);
                }
            }

            public void Clear(string key)
            {
                var item = Items.FirstOrDefault(i => i.Key == key);
                if (item == null) return;
                item.Data.Clear();
                Items.Remove(item);
            }

            public void ClearAll()
            {
                Items.Clear();
            }

            public bool Contains(string key, string globalObjectId)
            {
                var item = Items.FirstOrDefault(i => i.Key == key);
                if (item == null) return false;
                var data = GetData(key, globalObjectId);
                return data != null;
            }

            public UnpackData GetData(string key, string globalObjectId)
            {
                var item = Items.FirstOrDefault(i => i.Key == key);
                return item?.Data.FirstOrDefault(d => d.GlobalObjectId == globalObjectId);
            }

            public void RemoveData(string key, string globalObjectId)
            {
                var item = Items.FirstOrDefault(i => i.Key == key);
                if (item == null) return;
                var data = GetData(key, globalObjectId);
                if (data != null) item.Data.Remove(data);
            }

            #endregion
        }

        [Serializable]
        private class UnpackDataItem
        {
            #region ================================ FIELDS

            public string Key;
            public List<UnpackData> Data = new List<UnpackData>();

            #endregion
        }

        #endregion
    }
}

#endif