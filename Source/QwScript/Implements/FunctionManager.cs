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
