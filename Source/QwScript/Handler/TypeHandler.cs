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
using QwLua.Data;
using QwLua.Helpers;

namespace QwLua.Handler
{
    internal sealed class TypeHandler : BaseHandler
    {
        private Type _type;
        private string _fullPath;

        public TypeHandler(ScriptState luaState)
            : base(luaState)
        {
        }

        public Type Type
        {
            get { return _type; }
        }

        public override void Initilaze(object data)
        {
            var objs = data as object[];
            if (objs != null)
            {
                _type = (Type)objs[0];
                _fullPath = (string)objs[1];
            }
        }

        public override int Reg2Env()
        {
            ScriptHelper.SearchSetLuaObjectEx(State, _fullPath, Type, ScriptHelper.GetMetatableNameByType(Type));
            return 1;
        }
    }
}