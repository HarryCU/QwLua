
namespace QwLua.Reflection
{
    public interface IConstructor : IMember
    {
        object Inovke(params object[] @params);
    }
}
