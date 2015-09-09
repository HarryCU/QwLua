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
using QwLua.Event;

namespace QwLua.Implements
{
    internal static class HookManagerPool
    {
        public static class Functioner
        {
            public static readonly DotNetHookHandler HookHandler = Callback;

            private static void Callback(ScriptState luaState, HookInfo info)
            {
                var hookMgr = GetManager(luaState);
                if (hookMgr == null) return;

                if (hookMgr.Disposed) return;
                try
                {
                    hookMgr.TriggerHook(new HookEventArgs(luaState, info));
                }
                catch (Exception ex)
                {
                    hookMgr.TriggerError(new ErrorEventArgs(ex));
                }
            }
        }

        private static readonly IDictionary<IntPtr, HookManager> Pools = new Dictionary<IntPtr, HookManager>(10);

        public static void Add(ScriptState luaState, HookManager hookManager)
        {
            if (!Pools.ContainsKey(luaState))
            {
                Pools.Add(luaState, hookManager);
            }
        }

        public static HookManager GetManager(ScriptState luaState)
        {
            if (Pools.ContainsKey(luaState))
            {
                return Pools[luaState];
            }
            return null;
        }

        public static void Remove(ScriptState luaState)
        {
            IntPtr ptr = luaState;
            if (Pools.ContainsKey(ptr))
            {
                Pools.Remove(ptr);
            }
        }

        public static void SetGlobalHookCallback()
        {
            LuaCore.SetDotNetHook(Functioner.HookHandler);
        }
    }
}
