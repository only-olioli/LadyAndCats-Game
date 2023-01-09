#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace Settings.Editor
{
    [CustomEditor(typeof(GameSettings))]
    public sealed class GameSettingsEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            if (GUILayout.Button("Open Editor"))
            {
                EditorWindow.GetWindow<SettingsEditor>(SettingsEditor.EDITOR_NAME);
            }
        }
    }
}
#endif
