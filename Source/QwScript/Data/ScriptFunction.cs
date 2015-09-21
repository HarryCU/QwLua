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
            _reference = LuaCore.GetRef(luaState, reference);
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