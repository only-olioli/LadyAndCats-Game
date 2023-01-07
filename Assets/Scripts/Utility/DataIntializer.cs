using System;

namespace Utility
{
    public sealed class DataIntializer<T> : IInitializer, IValueGetter<T>
    {
        private readonly Initializer _initializer;
        private T _data;

        public DataIntializer(Func<T> initialize, Action<T> postInit = null)
        {
            _initializer = new(() =>
            {
                _data = initialize();
                postInit?.Invoke(_data);
            });
        }

        T IValueGetter<T>.Value => Get();

        public T Get()
        {
            _initializer.Initialize();
            return _data;
        }

        void IInitializer.Initialize() => _initializer.Initialize();

        public static implicit operator T(DataIntializer<T> dataIntializer) => dataIntializer.Get();
    }
}
