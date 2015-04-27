using System;

namespace QwLua.Exceptions
{
    public class LuaSourcetException : Exception
    {
        private readonly string _source;

        public override string Source
        {
            get { return _source; }
        }

        public LuaSourcetException(string message, string source)
            : base(message)
        {
            _source = source;
        }

        public LuaSourcetException(Exception ex, string source)
            : base("A .NET exception occured in user-code", ex)
        {
            _source = source;
        }
    }
}