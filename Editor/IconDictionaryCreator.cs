using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace SimpleFolderIcon.Editor
{
    public class IconDictionaryCreator : AssetPostprocessor
    {
        internal static Dictionary<string, Texture> IconDictionary;
        private const string PackageManagerIconPath = "Packages/com.corby-o.good-project-view/Editor/Icons";

        private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
        {
            if (!ContainsIconAsset(importedAssets) &&
                !ContainsIconAsset(deletedAssets) &&
                !ContainsIconAsset(movedAssets) &&
                !ContainsIconAsset(movedFromAssetPaths))
            {
                return;
            }

            BuildDictionary();
        }

        private static bool ContainsIconAsset(IEnumerable<string> assets)
        {
            return assets.Any(str => (ReplaceSeparatorChar(GetDirectoryName(str)) == PackageManagerIconPath));
        }

        private static string GetDirectoryName(string path)
        {
            return !Directory.Exists(path) ? string.Empty : Path.GetDirectoryName(path);
        }

        private static string ReplaceSeparatorChar(string path)
        {
            return path.Replace("\\", "/");
        }

        internal static void BuildDictionary()
        {
            var dictionary = new Dictionary<string, Texture>();
            var iconFolderPath = GetIconPath();
            var dir = new DirectoryInfo(iconFolderPath);
            var info = dir.GetFiles("*.png");
            foreach (var fileInfo in info)
            {
                var texture = (Texture)AssetDatabase.LoadAssetAtPath(Path.Combine(iconFolderPath, fileInfo.Name), typeof(Texture2D));
                dictionary.Add(Path.GetFileNameWithoutExtension(fileInfo.Name), texture);
            }

            IconDictionary = dictionary;
        }

        private static string GetIconPath()
        {
            if (Directory.Exists(PackageManagerIconPath))
            {
                return PackageManagerIconPath;
            }

            return string.Empty;
        }
    }
}
