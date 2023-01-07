#if UNITY_EDITOR
using System;
using UnityEditor;

namespace Utility.Editor
{
    public static class SerializedPropertyExtentions
    {
        private static void ThrowIfNotArray(this SerializedProperty property)
        {
            if (property is null)
            {
                throw new System.ArgumentNullException(nameof(property));
            }

            if (!property.IsArray())
            {
                throw new System.Exception(nameof(property) + " must be array!");
            }
        }

        public static bool IsArray(this SerializedProperty property) => property.propertyType != SerializedPropertyType.ArraySize || property.propertyType != SerializedPropertyType.Generic;

        public static void RemoveNullsFromArray(this SerializedProperty property)
        {
            property.ThrowIfNotArray();

            for (int i = 0; i < property.arraySize; i++)
            {
                SerializedProperty element = property.GetArrayElementAtIndex(i);
                if (element.objectReferenceValue == null)
                {
                    property.DeleteArrayElementAtIndex(i--);
                }
            }
        }

        public static bool Contains(this SerializedProperty property, Type type)
        {
            property.ThrowIfNotArray();
            for (int i = 0; i < property.arraySize; i++)
            {
                SerializedProperty element = property.GetArrayElementAtIndex(i);
                if (element.objectReferenceValue.GetType() == type)
                {
                    return true;
                }
            }
            return false;
        }

        public static void RemoveDuplicates(this SerializedProperty property, Func<SerializedProperty, SerializedProperty, bool> comparison)
        {
            property.ThrowIfNotArray();
            if (comparison is null)
            {
                throw new ArgumentNullException(nameof(comparison));
            }
            for (int i = property.arraySize - 1; i >= 1; i--)
            {
                if (IsDuplicated(i))
                {
                    property.DeleteArrayElementAtIndex(i);
                }
            }

            bool IsDuplicated(int i)
            {
                SerializedProperty value = property.GetArrayElementAtIndex(i);
                for (int j = i - 1; j >= 0; j--)
                {
                    SerializedProperty element = property.GetArrayElementAtIndex(j);
                    if (comparison(value, element))
                    {
                        return true;
                    }
                }
                return false;
            }
        }

    }
}
#endif
