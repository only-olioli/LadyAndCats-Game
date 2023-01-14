#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using Utility.Editor;
using Utility.Reflection;
using Utility.ScriptableObjects;

namespace Settings.Editor
{
    [InitializeOnLoad]
    internal static class SettingFilesController
    {
        private const string SettingsContainerName = "settings";

        private static readonly SettingsFileData _fileData;
        private static GameSettings _settings;
        public static ObjectPropertyEditor[] Editors { get; private set; }

        public static string AssetPath
        {
            get => _fileData.AssetPath;
            set
            {
                _fileData.AssetPath = value;
                Undo.RecordObject(_settings, "Change GameSettings assets path.");
                _settings.AssetPath = AssetPath;
                EditorUtility.SetDirty(_settings);
                AssetDatabase.SaveAssets();
            }
        }

        public static string SavePath
        {
            get => AssetDatabaseExtentions.FilePathToAssetPath(_settings.SavePath);
            set
            {
                Undo.RecordObject(_settings, "Change GameSettings save path.");
                _settings.SavePath = _fileData.SetSavePath(SavePath, value);
                EditorUtility.SetDirty(_settings);
                AssetDatabase.SaveAssets();
            }
        }

        static SettingFilesController()
        {
            _fileData = new();
            _settings = _fileData.GetSettings();
            InitSettings();
            EditorApplication.projectChanged += ProjectChangedHandle;
            Undo.undoRedoPerformed += UndoHandle;
        }

        private static void InitSettings()
        {
            SerializedObject obj = new(_settings);
            SerializedProperty settingsArray = obj.FindProperty(SettingsContainerName);
            settingsArray.RemoveNullsFromArray();
            Assembly assembly = Assembly.Load(nameof(Settings));
            Type[] settingsToAdd = assembly.GetTypes(t => IsSetting(t) && !settingsArray.Contains(t));
            int i = settingsArray.arraySize;
            settingsArray.arraySize += settingsToAdd.Length;
            ScriptableObjectExtentions.CreateAsChild(settingsToAdd, _settings, setting =>
            {
                setting.name = setting.GetType().Name;
                SerializedProperty addedSetting = settingsArray.GetArrayElementAtIndex(i++);
                addedSetting.objectReferenceValue = setting;
                addedSetting.isExpanded = true;
            }, false);
            settingsArray.RemoveDuplicates((a, b) =>
            {
                return a.objectReferenceValue.GetType() == b.objectReferenceValue.GetType();
            });
            if (obj.ApplyModifiedPropertiesWithoutUndo())
            {
                AssetDatabase.SaveAssets();
            }
            Editors = new ObjectPropertyEditor[settingsArray.arraySize];
            List<string> names = CreateEditors();

            _settings.RemoveAssets(names);

            List<string> CreateEditors()
            {
                int size = settingsArray.arraySize;
                List<string> names = new();
                for (int i = 0; i < size; i++)
                {
                    SerializedProperty property = settingsArray.GetArrayElementAtIndex(i);
                    Editors[i] = new(property);
                    names.Add(property.objectReferenceValue.name);
                }

                return names;
            }
        }

        private static void ProjectChangedHandle()
        {
            if (_settings == null)
            {
                _settings = _fileData.CreateNewSettings();
                Debug.LogWarning("You cannot delete Game Settings!", _settings);
                InitSettings();
                return;
            }

            AssetDatabase.Refresh();
            string assetPath = AssetDatabase.GetAssetPath(_settings);
            if (assetPath != _fileData.AssetPathFile)
            {
                _fileData.ResetPath(assetPath);
            }
            InitSettings();
        }

        private static void UndoHandle()
        {
            _fileData.ResetPath(_settings.AssetPath, SavePath);
        }

        private static bool IsSetting(Type type)
        {
            return type.ImplementsInterface<ISetting>() &&
                !type.IsAbstract &&
                type.IsChildOf(typeof(ScriptableObject));
        }
    }
}
#endif
