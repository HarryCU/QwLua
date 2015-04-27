using QwLua.Data;
using QwLua.Implements;

namespace QwLua
{
    public static class LuaGC
    {
        public static void Collect(ScriptState state, GCOptions gcOptions)
        {
            Collect(state, gcOptions, 0);
        }

        public static void Collect(ScriptState state, GCOptions gcOptions, int data)
        {
            LuaCore.GC(state, gcOptions, data);
        }
    }
}
