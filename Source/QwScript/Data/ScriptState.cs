using System;

namespace QwLua.Data
{
    public struct ScriptState
    {
        public ScriptState(IntPtr ptrState)
        {
            _state = ptrState;
        }

        public static implicit operator ScriptState(IntPtr ptr)
        {
            return new ScriptState(ptr);
        }

        public static implicit operator IntPtr(ScriptState luaState)
        {
            return luaState._state;
        }

        private readonly IntPtr _state;
    }

    public struct LuaDebug
    {
        public LuaDebug(IntPtr ptrDebug)
        {
            _debug = ptrDebug;
        }

        public static implicit operator LuaDebug(IntPtr ptr)
        {
            return new LuaDebug(ptr);
        }

        public static implicit operator IntPtr(LuaDebug luaDebug)
        {
            return luaDebug._debug;
        }

        private readonly IntPtr _debug;
    }
}