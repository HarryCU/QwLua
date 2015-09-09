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

using System.Runtime.InteropServices;
using QwLua.Data;
using QwLua.Helpers;
using QwLua.Implements;

namespace QwLua.Metatable
{
    internal sealed class UserDataMetatable : BaseMetatable
    {
        #region Functioner

        public static class Functioner
        {
            public readonly static ScriptFunctionHandler Gc = GcInternal;
            public new static readonly ScriptFunctionHandler ToString = ToStringInternal;

            [AllowReversePInvokeCalls]
            private static int GcInternal(ScriptState luaState)
            {
                int index;
                var runtime = LuaRuntimePool.GetRuntime(luaState);
                runtime.ObjectMgr.TryGetObject(luaState, 1, out index);
                if (index >= 0)
                {
                    runtime.ObjectMgr.Remove(luaState, index);
                }
                return 0;
            }

            [AllowReversePInvokeCalls]
            private static int ToStringInternal(ScriptState luaState)
            {
                var toString = string.Empty;
                var data = DataHelper.GetNetObject(luaState, 1);
                if (data != null)
                {
                    toString = data.ToString();
                }
                LuaCore.PushString(luaState, toString);
                return 1;
            }
        }

        #endregion

        public const string GlobalName = "dotNet_userDataMetatable";

        public UserDataMetatable(ScriptState luaState)
            : base(luaState)
        {
        }

        public override int Reg2Env()
        {
            var l = State;
            LuaCore.NewMetatable(l, GlobalName);
            LuaCore.PushString(l, "__gc");
            LuaCore.PushInvokeMethod(l, Functioner.Gc);
            LuaCore.SetTable(l, -3);
            LuaCore.PushString(l, "__tostring");
            LuaCore.PushInvokeMethod(l, Functioner.ToString);
            LuaCore.SetTable(l, -3);
            LuaCore.SetTop(l, -2);
            return 1;
        }
    }
}
