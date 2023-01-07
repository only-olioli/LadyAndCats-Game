using System;

namespace Utility.Reflection
{
    public static class TypeExtentions
    {
        public static bool IsChildOf(this Type type, Type parent)
        {
            while (type is not null)
            {
                if (type == parent)
                {
                    return true;
                }
                type = type.BaseType;
            }
            return false;
        }

        public static bool ImplementsInterface<T>(this Type type) => type.GetInterface(typeof(T).Name) is not null;
    }
}
