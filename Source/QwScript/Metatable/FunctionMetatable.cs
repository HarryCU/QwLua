using System.Runtime.InteropServices;
using QwLua.Data;
using QwLua.Helpers;
using QwLua.Implements;

namespace QwLua.Metatable
{
    internal sealed class FunctionMetatable : BaseMetatable
    {
        #region Functioner

        public static class Functioner
        {
            public readonly static ScriptFunctionHandler Gc = GcInternal;
            public readonly static ScriptFunctionHandler Call = CallInternal;

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
            private static int CallInternal(ScriptState luaState)
            {
                var func = DataHelper.GetNetObject<ScriptFunctionHandler>(luaState, 1);
                if (func != null)
                {
                    LuaCore.RemoveValue(luaState, 1);
                    return func(luaState);
                }
                LuaCore.PushNull(luaState);
                return 1;
            }
        }

        #endregion

        public const string GlobalName = "dotNet_functionMetatable";

        public FunctionMetatable(ScriptState luaState)
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
            LuaCore.PushString(l, "__call");
            LuaCore.PushInvokeMethod(l, Functioner.Call);
            LuaCore.SetTable(l, -3);
            LuaCore.SetTop(l, -2);
            return 1;
        }
    }
}
