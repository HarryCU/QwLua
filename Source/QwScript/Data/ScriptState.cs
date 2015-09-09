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

namespace QwLua.Data
{
    public struct ScriptState
    {
        public ScriptState(IntPtr ptrState)
        {
            _state = ptrState;
        }

        public static implicit operator ScriptState(IntPtr ptr)
        {
            return new ScriptState(ptr);
        }

        public static implicit operator IntPtr(ScriptState luaState)
        {
            return luaState._state;
        }

        private readonly IntPtr _state;
    }

    public struct LuaDebug
    {
        public LuaDebug(IntPtr ptrDebug)
        {
            _debug = ptrDebug;
        }

        public static implicit operator LuaDebug(IntPtr ptr)
        {
            return new LuaDebug(ptr);
        }

        public static implicit operator IntPtr(LuaDebug luaDebug)
        {
            return luaDebug._debug;
        }

        private readonly IntPtr _debug;
    }
}