using System;

namespace QwLua.Attributes
{
    [AttributeUsage(AttributeTargets.All, AllowMultiple = false, Inherited = true)]
    public class ScriptIgnoreAttribute : Attribute
    {
    }
}
