using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Utility
{
    public static class GameObjectExtentions
    {
        public static T CreateInstance<T>(bool dontDestroyOnLoad = false, string name = null) where T : Component
        {
            Type type = typeof(T);
            GameObject instance = new(name ?? type.Name, type);
            if (dontDestroyOnLoad)
            {
                Object.DontDestroyOnLoad(instance);
            }
            return instance.GetComponent<T>();
        }
    }
}
