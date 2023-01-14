#if UNITY_EDITOR
using System;
using System.IO;
using UnityEditor;
using UnityEngine;
using Utility.Editor;
using Utility.IO;
using Utility.ScriptableObjects;

namespace Settings.Editor
{
    internal sealed class SettingsFileData
    {
        private const string ASSETS = "Assets";
        private const string META = ".meta";
        private const string RESOURCES = nameof(Resources);
        public const string GAME_SETTINGS_FILE = "/" + SettingsEditor.GAME_SETTINGS + ".asset";
        private const string DEFAULT_ASSET_PATH = ASSETS + "\\Resources";
        private readonly SlashPath _assetPath = new() { Path = DEFAULT_ASSET_PATH};
        private readonly SlashPath _savePath = new();
        private bool _isMoving;

        public string AssetPath
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
                string oldPath = AssetPathFile;
                _assetPath.Path = value;
                _isMoving = true;
                AssetDatabase.MoveAsset(oldPath, AssetPathFile);
                AssetDatabase.Refresh();
                _isMoving = false;
            }
        }

        public string AssetPathFile => AssetPath + GAME_SETTINGS_FILE;

        public GameSettings GetSettings()
        {
            var settings = Resources.Load<GameSettings>(SettingsEditor.GAME_SETTINGS) ?? CreateNewSettings();
            if (string.IsNullOrWhiteSpace(settings.AssetPath))
            {
                settings.AssetPath = DEFAULT_ASSET_PATH;
            }
            _assetPath.Path = settings.AssetPath;
            _savePath.Path = AssetDatabaseExtentions.FilePathToAssetPath(settings.SavePath);
            return settings;
        }

        public GameSettings CreateNewSettings()
        {
            DirectoryExtentions.TryCreate(AssetPath);
            return ScriptableObjectExtentions.CreateInstance<GameSettings>(AssetPathFile);
        }

        public void ResetPath(string path)
        {
            if (_isMoving)
            {
                return;
            }

            Debug.LogWarning("You can edit Game Settings only with Settings Editor!");
            AssetDatabase.MoveAsset(path, AssetPathFile);
            AssetDatabase.Refresh();
        }

        public void ResetPath(string assetPath, string savePath)
        {
            _isMoving = true;
            AssetDatabase.MoveAsset(AssetPathFile, assetPath + GAME_SETTINGS_FILE);
            AssetDatabase.Refresh();
            _isMoving = false;
            _assetPath.Path = assetPath;

            MoveSave(_savePath, savePath);
        }

        public string SetSavePath(string oldValue, string newValue)
        {
            if (oldValue == newValue)
            {
                return oldValue.Remove(0, ASSETS.Length);
            }

            if (!newValue.Equals(ASSETS) &&
                !newValue.StartsWith(ASSETS + '\\') &&
                !newValue.StartsWith(ASSETS + '/')
                )
            {
                throw new Exception("Path must start with " + ASSETS + "!");
            }

            if (newValue.EndsWith('/') ||
                newValue.EndsWith('\\')
                )
            {
                throw new Exception("Path can't end with slash!");
            }

            MoveSave(oldValue, newValue);
            return newValue.Remove(0, ASSETS.Length);
        }

        private void MoveSave(string oldAssetPath, string newAssetPath)
        {
            string oldPath = new FileInfo(oldAssetPath + GameSettings.Save).FullName;
            if (File.Exists(oldPath))
            {
                string newDirectory = new DirectoryInfo(newAssetPath).FullName;
                DirectoryExtentions.TryCreate(newDirectory);
                File.Move(oldPath, newDirectory + GameSettings.Save);
                File.Move(oldPath + META, newDirectory + GameSettings.Save + META);
                AssetDatabase.Refresh();
            }
            _savePath.Path = newAssetPath;
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
