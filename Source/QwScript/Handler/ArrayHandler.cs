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
using QwLua.Helpers;
using QwLua.Implements;
using QwLua.Reflection;

namespace QwLua.Handler
{
    internal class ArrayHandler : BaseHandler, INewIndexer
    {
        private IMethod _method;
        private bool _getter;

        public ArrayHandler(ScriptState luaState)
            : base(luaState)
        {
        }

        public override void Initilaze(object data)
        {
            var objs = data as object[];
            if (objs != null)
            {
                _method = objs[0] as IMethod;
                _getter = (bool)objs[1];
            }
        }

        public override int Reg2Env()
        {
            var luaState = State;
            var runtime = LuaRuntimePool.GetRuntime(luaState);
            if (_getter)
            {
                var index = LuaCore.ToInt(luaState, -1);
                var val = _method.Invoke(Instance, index);
                runtime.ObjectMgr.PushObject(luaState, val);
                return 1;
            }
            DataHelper.PushObject(luaState, null);
            return 0;
        }

        public void SetValue(object value)
        {
            if (!_getter)
            {
                var index = LuaCore.ToInt(State, -2);
                _method.Invoke(Instance, index, value);
            }
        }
    }
}