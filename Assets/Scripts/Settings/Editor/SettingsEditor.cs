#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Callbacks;
using Utility.Editor;
using UnityEngine;

namespace Settings.Editor
{
    internal sealed class SettingsEditor : EditorWindow
    {
        public const string GAME_SETTINGS = nameof(GameSettings);
        public const string EDITOR_NAME = "Settings Editor";
        private const string ISETTING = nameof(ISetting);
        private const string OPEN_EDITOR = "Tools/Settings Editor";

        private string _path;
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
            _path = SettingFilesController.AssetPath;
            _error = string.Empty;
        }

        private void OnGUI()
        {
            DrawPath();

            foreach (ObjectPropertyEditor editor in SettingFilesController.Editors)
            {
                editor.OnInspectorGUIFoldout();
            }
        }

        private void DrawPath()
        {
            _path = EditorGUILayout.TextField("Path:", _path);
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Change Path"))
            {
                try
                {
                    SettingFilesController.AssetPath = _path;
                    _error = string.Empty;
                }
                catch (System.Exception e)
                {
                    _error = e.Message;
                }
            }
            if (GUILayout.Button("Current Path"))
            {
                _path = SettingFilesController.AssetPath;
            }
            EditorGUILayout.EndHorizontal();
            if (_error != string.Empty)
            {
                EditorGUILayout.HelpBox(_error, MessageType.Warning);
            }
        }
    }
}
#endif
