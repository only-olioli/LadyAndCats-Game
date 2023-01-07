#if UNITY_EDITOR
using UnityEditor;

namespace Utility.Editor
{
    public sealed class ObjectPropertyEditor
    {
        private readonly UnityEditor.Editor _editor;
        private readonly SerializedProperty _property;

        public ObjectPropertyEditor(SerializedProperty property)
        {
            _property = property ?? throw new System.ArgumentNullException(nameof(property));
            if (_property.propertyType != SerializedPropertyType.ObjectReference)
            {
                throw new System.Exception(nameof(property) + " must be type object reference!");
            }

            UnityEditor.Editor.CreateCachedEditor(_property.objectReferenceValue, null, ref _editor);
        }

        public void OnInspectorGUIFoldout(int indentLevel = 1)
        {
            if (_property.isExpanded = EditorGUILayout.Foldout(
                _property.isExpanded,
                _property.objectReferenceValue.name
                ))
            {
                OnInspectorGUI(indentLevel);
            }
        }

        public void OnInspectorGUI(int indentLevel = 1)
        {
            EditorGUI.indentLevel += indentLevel;
            _editor.OnInspectorGUI();
            EditorGUI.indentLevel -= indentLevel;
        }
    }
}
#endif
