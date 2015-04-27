using System;

namespace QwLua.Attributes
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class ScriptClassAttribute : Attribute
    {
        public string Name { get; private set; }

        public ScriptClassAttribute(string name)
        {
            Name = name;
        }
    }
}
