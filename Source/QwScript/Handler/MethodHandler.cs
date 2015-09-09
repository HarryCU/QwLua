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

using QwLua.Data;
using QwLua.Implements;
using QwLua.Metatable;
using QwLua.Wrapper;

namespace QwLua.Handler
{
    internal class MethodHandler : BaseHandler
    {
        private MethodFinder _methodFinder;

        public MethodHandler(ScriptState luaState)
            : base(luaState)
        {
        }

        public MethodFinder Finder
        {
            get { return _methodFinder; }
        }

        public override void Initilaze(object data)
        {
            _methodFinder = data as MethodFinder;
        }

        public override int Reg2Env()
        {
            var luaState = State;
            var runtime = LuaRuntimePool.GetRuntime(luaState);
            var wrapper = new ScriptFunctionHandler(new MethodWrapper(Finder).Invoker);
            runtime.ObjectMgr.PushObjectEx(luaState, wrapper, FunctionMetatable.GlobalName);
            return 1;
        }
    }
}