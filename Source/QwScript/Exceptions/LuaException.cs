using System;

namespace QwLua.Exceptions
{
    public class LuaException : Exception
    {
        public LuaException(string message)
            : base(message)
        {
        }

        public LuaException(string message, Exception innerExcpetion)
            : base(message, innerExcpetion)
        {
        }
    }
}