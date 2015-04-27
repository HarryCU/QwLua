using System.Collections.Generic;
using QwLua.Helpers;
using QwLua.Implements;

namespace QwLua.Data
{
    internal sealed class ScriptFunction : ScriptDataBase, IScriptFunction
    {
        private readonly int _reference;
        private readonly ScriptState _luaState;
        private readonly ScriptFunctionHandler _func;

        internal ScriptFunction(ScriptState luaState, int reference)
        {
            _reference = reference;
            _luaState = luaState;
        }

        internal ScriptFunction(ScriptState luaState, ScriptFunctionHandler func)
        {
            _luaState = luaState;
            _func = func;
            _reference = -1;
        }

        public IList<object> Call(params object[] args)
        {
            var topIndex = LuaCore.GetTop(_luaState);

            var runtime = LuaRuntimePool.GetRuntime(_luaState);
            if (_func != null)
            {
                DataHelper.PushObject(_luaState, _func);
            }
            else
            {
                LuaCore.GlobalRawGet(_luaState, _reference);
            }
            foreach (var arg in args)
            {
                DataHelper.PushObject(_luaState, arg);
            }
            if (LuaCore.Call(_luaState, args.Length, -1, 0) == 0)
                return runtime.ObjectMgr.PopResults(_luaState, topIndex);
            runtime.ThrowExceptionFromError(_luaState, topIndex);
            return null;
        }

        protected override void Dispose(bool disposing)
        {
        }
    }
}