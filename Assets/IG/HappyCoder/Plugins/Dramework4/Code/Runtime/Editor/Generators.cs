#if UNITY_EDITOR

using System.IO;

using IG.HappyCoder.Plugins.Dramework4.Runtime.Data;

using Newtonsoft.Json;

using UnityEditor;

using Task = System.Threading.Tasks.Task;


namespace IG.HappyCoder.Plugins.Dramework4.Runtime.Editor
{
    internal static partial class Generators
    {
        #region ================================ FIELDS

        private const string PATH_GENERATED_FOLDER = "Assets/Plugins/Dramework4/Runtime/Generated";
        private const string PATH_GENERATED_ASSEMBLY_DEFINITION_FILE = "Assets/Plugins/Dramework4/Runtime/Generated/_generated.asmdef";

        #endregion

        #region ================================ METHODS

        private static void CreateAssemblyDefinitionFile()
        {
            if (File.Exists(PATH_GENERATED_ASSEMBLY_DEFINITION_FILE)) return;
            CreateDirectory(PATH_GENERATED_FOLDER);
            var json = JsonConvert.SerializeObject(new AssemblyDefinition { name = "_generated" }, Formatting.Indented);
            File.WriteAllText(PATH_GENERATED_ASSEMBLY_DEFINITION_FILE, json);
        }

        private static void CreateDirectory(string path)
        {
            if (Directory.Exists(path)) return;
            Directory.CreateDirectory(path);
        }

        [InitializeOnLoadMethod]
        private static async void Initialize()
        {
            await Task.Delay(1000);

            if (EditorApplication.isPlayingOrWillChangePlaymode) return;

            CreateDirectory(PATH_GENERATED_FOLDER);
            CreateAssemblyDefinitionFile();
            GenerateIdentifiers();

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        #endregion
    }
}

#endif