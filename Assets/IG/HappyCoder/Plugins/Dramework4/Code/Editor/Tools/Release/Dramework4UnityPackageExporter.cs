using System;
using System.IO;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace HappyCoder.Dramework4.Editor.Tools.Release
{
    public static class Dramework4UnityPackageExporter
    {
        private const string PackageRoot = "Assets/IG";
        private const string DefaultOutputDirectory = "Builds/UnityPackage";

        [MenuItem("Tools/Dramework4/Release/Export UnityPackage")]
        public static void ExportFromMenu()
        {
            Export(DefaultOutputDirectory);
        }

        public static void ExportFromCommandLine()
        {
            var outputDirectory = GetArgumentValue("-dramework4ReleaseOutput") ?? DefaultOutputDirectory;
            Export(outputDirectory);
        }

        private static void Export(string outputDirectory)
        {
            var packageJsonPath = Path.Combine(PackageRoot, "package.json");
            if (!File.Exists(packageJsonPath))
            {
                throw new FileNotFoundException("Dramework4 package metadata was not found.", packageJsonPath);
            }

            var packageJson = File.ReadAllText(packageJsonPath);
            var version = ReadJsonString(packageJson, "version");
            var unity = ReadJsonString(packageJson, "unity");

            if (string.IsNullOrWhiteSpace(version) || string.IsNullOrWhiteSpace(unity))
            {
                throw new InvalidOperationException("Dramework4 package.json must define version and unity.");
            }

            Directory.CreateDirectory(outputDirectory);
            var artifactName = $"Dramework4-{version}-unity{unity}.unitypackage";
            var outputPath = Path.GetFullPath(Path.Combine(outputDirectory, artifactName));

            AssetDatabase.ExportPackage(
                new[] { PackageRoot },
                outputPath,
                ExportPackageOptions.Recurse);

            Debug.Log($"Dramework4 unitypackage exported: {outputPath}");
        }

        private static string GetArgumentValue(string name)
        {
            var args = Environment.GetCommandLineArgs();
            for (var i = 0; i < args.Length - 1; i++)
            {
                if (args[i].Equals(name, StringComparison.OrdinalIgnoreCase))
                {
                    return args[i + 1];
                }
            }

            return null;
        }

        private static string ReadJsonString(string json, string propertyName)
        {
            var match = Regex.Match(
                json,
                $"\"{Regex.Escape(propertyName)}\"\\s*:\\s*\"(?<value>[^\"]+)\"",
                RegexOptions.CultureInvariant);

            return match.Success ? match.Groups["value"].Value : null;
        }
    }
}
