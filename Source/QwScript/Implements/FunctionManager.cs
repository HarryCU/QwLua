using System;
using System.Collections.Generic;
using QwLua.Data;
using QwLua.Helpers;
using QwLua.Wrapper;

namespace QwLua.Implements
{
    public sealed class FunctionManager
    {
        private readonly ScriptState _luaState;

        internal FunctionManager(ScriptState luaState)
        {
            _luaState = luaState;
        }

        public IScriptFunction GetFunction(string fullPath)
        {
            return ScriptHelper.SearchLuaObject(_luaState, fullPath) as ScriptFunction;
        }

        public IList<object> Invoke(string fullPath, params object[] args)
        {
            var fn = GetFunction(fullPath);
            if (fn != null)
            {
                return fn.Call(args);
            }
            return null;
        }

        public IScriptFunction Register(string fullPath, Func<IList<object>, object> func)
        {
            var runtime = LuaRuntimePool.GetRuntime(_luaState);

            runtime[fullPath] = new ScriptFunctionHandler(new DelegateWrapper(func).Invoker);

            var luaFunc = runtime[fullPath] as ScriptFunctionHandler;
            return new ScriptFunction(_luaState, luaFunc);
        }
    }
}
