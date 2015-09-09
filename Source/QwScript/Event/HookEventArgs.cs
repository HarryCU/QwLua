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
