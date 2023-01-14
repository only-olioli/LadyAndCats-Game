namespace Utility
{
    public interface IValueSetter<in T>
    {
        T Value { set; }
    }
}
