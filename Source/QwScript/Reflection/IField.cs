
namespace QwLua.Reflection
{
    public interface IField : IMember
    {
        bool IsStatic { get; }
        void SetValue(object instance, object value);
        object GetValue(object instance);
    }
}
