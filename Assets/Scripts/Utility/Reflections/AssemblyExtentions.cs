using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace Utility.Reflection
{
    public static class AssemblyExtentions
    {
        private static readonly List<Type> _types = new();

        public static Type[] GetTypes(this Assembly assembly, Predicate<Type> predicate)
        {
            if (predicate is null)
            {
                throw new ArgumentNullException(nameof(predicate));
            }

            _types.Clear();
            foreach (Type type in assembly.GetTypes())
            {
                if (predicate(type))
                {
                    _types.Add(type);
                }
            }
            return _types.ToArray();
        }

        
    }
}
