using System;
using System.Runtime.InteropServices;
using QwLua.Data;
using QwLua.Helpers;
using QwLua.Implements;

namespace QwLua.Metatable
{
    internal sealed class EnumMetatable : BaseMetatable
    {
        #region Functioner

        public static class Functioner
        {
            public new static readonly ScriptFunctionHandler ToString = ToStringInternal;
            public static readonly ScriptFunctionHandler Index = IndexInternal;

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

            [AllowReversePInvokeCalls]
            private static int IndexInternal(ScriptState luaState)
            {
                var type = DataHelper.GetNetObject<Type>(luaState, 1);
                var enumFieldName = LuaCore.ToString(luaState, -1);
                if (!string.IsNullOrEmpty(enumFieldName) && type != null && type.IsEnum)
                {
                    LuaCore.RemoveValue(luaState, 1);
                    var runtime = LuaRuntimePool.GetRuntime(luaState);
                    var value = Enum.Parse(type, enumFieldName);
                    runtime.ObjectMgr.PushObject(luaState, value);
                    return 1;
                }
                LuaCore.PushNull(luaState);
                return 1;
            }
        }

        #endregion

        public const string GlobalName = "dotNet_enumMetatable";

        public EnumMetatable(ScriptState luaState)
            : base(luaState)
        {
        }

        public override int Reg2Env()
        {
            var l = State;
            LuaCore.NewMetatable(l, GlobalName);
            LuaCore.PushString(l, "__tostring");
            LuaCore.PushInvokeMethod(l, Functioner.ToString);
            LuaCore.SetTable(l, -3);
            LuaCore.PushString(l, "__index");
            LuaCore.PushInvokeMethod(l, Functioner.Index);
            LuaCore.SetTable(l, -3);
            LuaCore.SetTop(l, -2);
            return 1;
        }
    }
}