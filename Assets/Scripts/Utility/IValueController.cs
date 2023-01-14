using System;

namespace Utility
{
    public interface IValueController<T> : IValueHolder<T>
    {
        event Action<T> OnValueChanged;
    }
}
