
using System.Reflection;

namespace QwLua.Reflection
{
    public interface IMember
    {
        string Name { get; }
        MemberInfo Member { get; }
    }
}
