#if UNITY_EDITOR
using UnityEditor;

namespace Settings.Editor
{
    [CustomEditor(typeof(Resolution))]
    public sealed class ResolutionEditor : UnityEditor.Editor
    {
        UnityEditor.Editor _baseEditor;

        private void OnEnable()
        {
            CreateCachedEditor(target, typeof(SettingEditor), ref _baseEditor);
        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.Popup("Available resolutions", -1, Resolution.Resolutions.ToArray());
            _baseEditor.OnInspectorGUI();
        }
    }
}
#endif
