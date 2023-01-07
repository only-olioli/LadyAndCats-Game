namespace Utility
{
    public interface IValueGetter<out T>
    {
        T Value { get; }
    }
}