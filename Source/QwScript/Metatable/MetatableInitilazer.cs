using System;
using System.Collections.Generic;
using QwLua.Data;

namespace QwLua.Metatable
{
    public class MetatableInitilazer
    {
        private static readonly IList<Func<ScriptState, ILuaRegister>> Registers = new List<Func<ScriptState, ILuaRegister>>
        {
            luaState => new DotNetObjectsMetatable(luaState),
            luaState => new ObjectMetatable(luaState),
            luaState => new FunctionMetatable(luaState),
            luaState => new EnumMetatable(luaState),
            luaState => new UserDataMetatable(luaState)
        };

        private readonly ScriptState _luaState;

        public MetatableInitilazer(ScriptState luaState)
        {
            _luaState = luaState;
        }

        public void Initilaze()
        {
            foreach (var func in Registers)
            {
                var luaRegister = func(_luaState);
                luaRegister.Reg2Env();
            }
        }
    }
}