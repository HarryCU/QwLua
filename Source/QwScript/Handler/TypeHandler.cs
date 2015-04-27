using System;
using QwLua.Data;
using QwLua.Helpers;

namespace QwLua.Handler
{
    internal sealed class TypeHandler : BaseHandler
    {
        private Type _type;
        private string _fullPath;

        public TypeHandler(ScriptState luaState)
            : base(luaState)
        {
        }

        public Type Type
        {
            get { return _type; }
        }

        public override void Initilaze(object data)
        {
            var objs = data as object[];
            if (objs != null)
            {
                _type = (Type)objs[0];
                _fullPath = (string)objs[1];
            }
        }

        public override int Reg2Env()
        {
            ScriptHelper.SearchSetLuaObjectEx(State, _fullPath, Type, ScriptHelper.GetMetatableNameByType(Type));
            return 1;
        }
    }
}