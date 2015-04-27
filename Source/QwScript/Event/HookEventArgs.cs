using System;
using QwLua.Data;

namespace QwLua.Event
{
    public class HookEventArgs : EventArgs
    {
        private readonly HookInfo _info;
        private readonly ScriptState _luaState;

        public HookEventArgs(ScriptState luaState, HookInfo info)
        {
            _luaState = luaState;
            _info = info;
        }

        public HookInfo Info
        {
            get { return _info; }
        }

        public ScriptState LuaState { get { return _luaState; } }
    }
}
