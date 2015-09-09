/**
 * Copyright (c) 2015, Harry CU 邱允根 (292350862@qq.com).
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *      http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

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
