namespace Utility
{
    public interface IValueHolder<T> : IValueGetter<T>, IValueSetter<T>
    {
        new T Value { get; set; }

        T IValueGetter<T>.Value => Value;

        T IValueSetter<T>.Value
        {
            set => Value = value;
        }
    }
}
