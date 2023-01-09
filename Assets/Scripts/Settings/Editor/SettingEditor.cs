#if UNITY_EDITOR
using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Settings.Editor
{
    [CustomEditor(typeof(Setting<,>), true)]
    public sealed class SettingEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            Type settingType = target.GetType();
            Type parent = settingType.BaseType;
            while (parent.BaseType != typeof(ScriptableObject))
            {
                parent = parent.BaseType;
            }
            MethodInfo getValue = parent.GetMethod("get_Value", BindingFlags.Static | BindingFlags.Public);
            GUI.enabled = false;
            EditorGUILayout.TextField("Current value: ", getValue.Invoke(null, null).ToString());
            DrawSavedValue();
            GUI.enabled = true;
        }

        private void DrawSavedValue()
        {
            SettingsSave.NameValue nameValue = SettingsSave.GetSettingValue((ISetting)target);
            string saved = "-";
            if (nameValue is not null)
            {
                MethodInfo getValue = nameValue.GetType().GetMethod("get_Value", BindingFlags.Instance | BindingFlags.Public);
                saved = getValue.Invoke(nameValue, null).ToString();
            }
            EditorGUILayout.TextField("Saved value: ", saved);
        }
    }
}
#endif
