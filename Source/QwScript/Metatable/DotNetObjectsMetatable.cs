using QwLua.Data;
using QwLua.Implements;

namespace QwLua.Metatable
{
    internal class DotNetObjectsMetatable : BaseMetatable
    {
        public const string GlobalName = "dotNet_objects";

        public DotNetObjectsMetatable(ScriptState luaState)
            : base(luaState)
        {
        }

        public override int Reg2Env()
        {
            var luaState = State;
            LuaCore.PushString(luaState, GlobalName);
            LuaCore.CreateTable(luaState);
            LuaCore.CreateTable(luaState);
            LuaCore.PushString(luaState, "__mode");
            LuaCore.PushString(luaState, "v");
            LuaCore.SetTable(luaState, -3);
            LuaCore.SetMetatable(luaState, -2);
            LuaCore.SetGlobalTable(luaState);
            return 1;
        }
    }
}
