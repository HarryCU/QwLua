using System;
using System.Collections.Generic;
using QwLua.Data;

namespace QwLua.Implements
{
    internal static class LuaRuntimePool
    {
        private static readonly IDictionary<IntPtr, LuaRuntime> Pools = new Dictionary<IntPtr, LuaRuntime>(10);

        public static void Add(ScriptState luaState, LuaRuntime runtime)
        {
            if (!Pools.ContainsKey(luaState))
            {
                Pools.Add(luaState, runtime);
            }
        }

        public static LuaRuntime GetRuntime(ScriptState luaState)
        {
            if (Pools.ContainsKey(luaState))
            {
                return Pools[luaState];
            }
            return null;
        }

        public static void Remove(ScriptState luaState)
        {
            IntPtr ptr = luaState;
            if (Pools.ContainsKey(ptr))
            {
                Pools.Remove(ptr);
            }
        }
    }
}
