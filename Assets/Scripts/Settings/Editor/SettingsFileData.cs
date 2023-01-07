#if UNITY_EDITOR
using System;
using System.IO;
using UnityEditor;
using UnityEngine;
using Utility.IO;
using Utility.ScriptableObjects;

namespace Settings.Editor
{
    internal sealed class SettingsFileData
    {
        private const string ASSETS = "Assets";
        private const string RESOURCES = nameof(Resources);
        public const string GAME_SETTINGS_FILE = "/" + SettingsEditor.GAME_SETTINGS + ".asset";
        private readonly string _defaultAssetPath = ASSETS + "\\Resources";
        private readonly SlashPath _assetPath = new();
        private bool _isMoving;

        public string Path
        {
            get => _assetPath;
            set
            {
                if (value == _assetPath)
                {
                    return;
                }

                if (!value.StartsWith(ASSETS + '\\') &&
                    !value.StartsWith(ASSETS + '/')
                    )

                {
                    throw new Exception("Path must start with " + ASSETS + "!");
                }

                if (!value.EndsWith('\\' + RESOURCES) &&
                    !value.EndsWith('/' + RESOURCES))
                {
                    throw new Exception("Path must start with " + RESOURCES + "!");
                }
                
                DirectoryExtentions.TryCreate(value);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
                string oldPath = PathFile;
                _assetPath.Path = value;
                _isMoving = true;
                Debug.Log(AssetDatabase.MoveAsset(oldPath, PathFile));
                AssetDatabase.Refresh();
                _isMoving = false; 
            }
        }

        public string PathFile => Path + GAME_SETTINGS_FILE;

        public GameSettings GetSettings()
        {
            string assetsPath = Application.dataPath;
            string path = FileExtentions.FindFilePath(SettingsEditor.GAME_SETTINGS + ".asset", assetsPath);
            if (path is null)
            {
                _assetPath.Path = _defaultAssetPath;
                return CreateNewSettings();
            }

            DirectoryInfo info = new DirectoryInfo(path).Parent;
            DirectoryInfo assetsPathInfo = new(assetsPath);
            string relativeAssetsPath = info.Name;

            while (info.FullName != assetsPathInfo.FullName)
            {
                info = info.Parent;
                relativeAssetsPath = info.Name + '\\' + relativeAssetsPath;
            }

            _assetPath.Path = relativeAssetsPath;
            return Resources.Load<GameSettings>(SettingsEditor.GAME_SETTINGS);
        }

        public GameSettings CreateNewSettings()
        {
            DirectoryExtentions.TryCreate(Path);
            return ScriptableObjectExtentions.CreateInstance<GameSettings>(PathFile);
        }

        public void ResetPath(string path)
        {
            if (_isMoving)
            {
                return;
            }
            
            Debug.LogWarning("You can edit Game Settings only with Settings Editor!");
            AssetDatabase.MoveAsset(path, PathFile);
            AssetDatabase.Refresh();
        }

        private class SlashPath
        {
            private string _path;

            public string Path { get => _path; set => _path = value.Replace('\\', '/'); }

            public static implicit operator string(SlashPath path) => path._path;
        }
    }
}
#endif
