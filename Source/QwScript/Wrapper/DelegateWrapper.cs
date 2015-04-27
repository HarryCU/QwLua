using System;
using System.Collections.Generic;
using QwLua.Data;
using QwLua.Helpers;
using QwLua.Implements;

namespace QwLua.Wrapper
{
    internal sealed class DelegateWrapper
    {
        private readonly Func<IList<object>, object> _func;
        private readonly ScriptFunctionHandler _functionHandler;

        public DelegateWrapper(Func<IList<object>, object> func)
        {
            _func = func;
            _functionHandler = Call;
        }

        private int Call(ScriptState luaState)
        {
            var runtime = LuaRuntimePool.GetRuntime(luaState);
            var n = LuaCore.GetTop(luaState);
            IList<object> args = new List<object>(n);
            for (int i = 1; i <= n; i++)
            {
                args.Add(DataHelper.GetObject(luaState, i));
            }
            object result = null;
            try
            {
                result = _func(args);
            }
            catch (Exception ex)
            {
                runtime.ThrowError(ex);
            }
            DataHelper.PushObject(luaState, result);
            return 1;
        }

        public ScriptFunctionHandler Invoker
        {
            get { return _functionHandler; }
        }
    }
}
