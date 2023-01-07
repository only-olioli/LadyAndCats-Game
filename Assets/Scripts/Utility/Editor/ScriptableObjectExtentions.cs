#if UNITY_EDITOR
using System;
using UnityEditor;
using UnityEngine;

namespace Utility.ScriptableObjects
{
    public static class ScriptableObjectExtentions
    {
        public static T CreateInstance<T>(string assetPath, bool saveAssets = true) where T : ScriptableObject
        {
            T instance = ScriptableObject.CreateInstance<T>();
            AssetDatabase.CreateAsset(instance, assetPath);
            if (saveAssets)
            {
                AssetDatabase.SaveAssets();
            }
            return instance;
        }

        public static void CreateAsChild(Type[] types, ScriptableObject parent, Action<ScriptableObject> proceed, bool saveAssets = true)
        {
            foreach (Type type in types)
            {
                ScriptableObject instance = ScriptableObject.CreateInstance(type);
                AssetDatabase.AddObjectToAsset(instance, parent);
                proceed?.Invoke(instance);
            }
            if (saveAssets)
            {
                AssetDatabase.SaveAssets();
            }
        }
    }
}
#endif
