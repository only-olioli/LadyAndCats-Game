#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Callbacks;
using Utility.Editor;
using UnityEngine;
using System;

namespace Settings.Editor
{
    internal sealed class SettingsEditor : EditorWindow
    {
        public const string GAME_SETTINGS = nameof(GameSettings);
        public const string EDITOR_NAME = "Settings Editor";
        private const string ISETTING = nameof(ISetting);
        private const string OPEN_EDITOR = "Tools/Settings Editor";

        private string _assetPath;
        private string _savePath;
        private string _error;

        [MenuItem(OPEN_EDITOR)]
        public static void ShowWindow()
        {
            GetWindow<SettingsEditor>(EDITOR_NAME);
        }

        [OnOpenAsset()]
        public static bool ShowWindow(int instanceID, int _)
        {
            if (EditorUtility.InstanceIDToObject(instanceID).GetType() == typeof(GameSettings))
            {
                ShowWindow();
                return true;
            }
            return false;
        }

        private void OnEnable()
        {
            _assetPath = SettingFilesController.AssetPath;
            _savePath = SettingFilesController.SavePath;
            _error = string.Empty;
        }

        private void OnGUI()
        {
            DrawPathChange("Asset Path: ", ref _assetPath, path => SettingFilesController.AssetPath = path, () => SettingFilesController.AssetPath);
            DrawPathChange("Save Path: ", ref _savePath, path => SettingFilesController.SavePath = path, () => SettingFilesController.SavePath);

            if (_error != string.Empty)
            {
                EditorGUILayout.HelpBox(_error, MessageType.Warning);
            }

            foreach (ObjectPropertyEditor editor in SettingFilesController.Editors)
            {
                editor.OnInspectorGUIFoldout();
            }
        }

        private void DrawPathChange(string label, ref string text, Action<string> setPath, Func<string> getPath)
        {
            text = EditorGUILayout.TextField(label, text);
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Change Path"))
            {
                try
                {
                    setPath(text);
                    _error = string.Empty;
                }
                catch (System.Exception e)
                {
                    _error = e.Message;
                }
            }
            if (GUILayout.Button("Current Path"))
            {
                text = getPath();
            }
            EditorGUILayout.EndHorizontal();
        }
    }
}
#endif
