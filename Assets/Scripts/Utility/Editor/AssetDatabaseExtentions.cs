#if UNITY_EDITOR
using System.IO;
using UnityEngine;

namespace Utility.Editor
{
    public static class AssetDatabaseExtentions
    {
        public static string FilePathToAssetPath(string path)
        {
            DirectoryInfo info = new DirectoryInfo(path).Parent;
            DirectoryInfo assetsPathInfo = new(Application.dataPath);
            string relativeAssetsPath = info.Name;
            while (info.FullName != assetsPathInfo.FullName)
            {
                info = info.Parent;
                relativeAssetsPath = info.Name + '\\' + relativeAssetsPath;
            }
            return relativeAssetsPath;
        }
    }
}
#endif
