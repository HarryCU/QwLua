using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using QwLua.Data;
using QwLua.Handler;
using QwLua.Helpers;
using QwLua.Implements;
using QwLua.Reflection;

namespace QwLua.Metatable
{
    internal sealed class ObjectMetatable : BaseMetatable
    {
        #region Functioner

        public static class Functioner
        {
            public static readonly ScriptFunctionHandler Gc = GcInternal;
            public new static readonly ScriptFunctionHandler ToString = ToStringInternal;
            public static readonly ScriptFunctionHandler Index = IndexInternal;
            public static readonly ScriptFunctionHandler NewIndex = NewIndexInternal;
            public static readonly ScriptFunctionHandler Call = CallInternal;

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

            [AllowReversePInvokeCalls]
            private static int IndexInternal(ScriptState luaState)
            {
                var data = DataHelper.GetNetObject(luaState, 1);
                var memberName = LuaCore.ToString(luaState, -1);
                if (!string.IsNullOrEmpty(memberName) && data != null)
                {
                    LuaCore.RemoveValue(luaState, 1);
                    var handler = GetArrayHandler(luaState, -1, data, true);
                    if (handler == null)
                        handler = GetMember(luaState, data, memberName);
                    if (handler != null)
                    {
                        return handler.Reg2Env();
                    }
                }
                LuaCore.PushNull(luaState);
                return 1;
            }

            [AllowReversePInvokeCalls]
            private static int NewIndexInternal(ScriptState luaState)
            {
                var key = DataHelper.GetObject(luaState, -2);
                var value = DataHelper.GetObject(luaState, -1);
                var data = DataHelper.GetNetObject(luaState, 1);
                if (data != null && key != null)
                {
                    var handler = GetArrayHandler(luaState, -2, data, false) as INewIndexer;
                    if (handler == null)
                    {
                        var k = key as string;
                        if (k != null)
                            handler = GetMember(luaState, data, k) as INewIndexer;
                    }
                    if (handler != null)
                    {
                        handler.SetValue(value);
                    }
                }
                return 0;
            }

            [AllowReversePInvokeCalls]
            private static int CallInternal(ScriptState luaState)
            {
                var type = DataHelper.GetNetObject<Type>(luaState, 1);
                if (type != null)
                {
                    LuaCore.RemoveValue(luaState, 1);
                    var runtime = LuaRuntimePool.GetRuntime(luaState);
                    var args = GetCtorArguments(luaState);
                    int argCount = args.Count;
                    var instance = type.New(args.ToArray());
                    args.Clear();
                    if (instance == null)
                    {
                        #region Show Error

                        var builder = new StringBuilder(256);
                        builder.Append("引擎未找到");
                        builder.Append(type.FullName).Append("(");
                        int i = 0;
                        foreach (var o in args)
                        {
                            builder.Append(o.GetType().FullName);
                            if (i != argCount - 1)
                                builder.Append(",");
                            i++;
                        }
                        builder.Append(")构造函数");
                        runtime.ThrowError(builder.ToString());
                        builder.Clear();

                        #endregion
                    }
                    else
                    {
                        runtime.ObjectMgr.PushObjectEx(luaState, instance, GlobalName);
                        return 1;
                    }
                }
                LuaCore.PushNull(luaState);
                return 1;
            }

            private static IList<object> GetCtorArguments(ScriptState luaState)
            {
                var n = LuaCore.GetTop(luaState);
                IList<object> args = new List<object>(n);
                for (int i = 1; i <= n; i++)
                {
                    args.Add(DataHelper.GetObject(luaState, i));
                }
                return args;
            }
        }

        #endregion

        public const string GlobalName = "dotNet_objectMetatable";

        public ObjectMetatable(ScriptState luaState)
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
            LuaCore.PushString(l, "__index");
            LuaCore.PushInvokeMethod(l, Functioner.Index);
            LuaCore.SetTable(l, -3);
            LuaCore.PushString(l, "__newindex");
            LuaCore.PushInvokeMethod(l, Functioner.NewIndex);
            LuaCore.SetTable(l, -3);
            LuaCore.PushString(l, "__call");
            LuaCore.PushInvokeMethod(l, Functioner.Call);
            LuaCore.SetTable(l, -3);
            LuaCore.SetTop(l, -2);
            return 1;
        }
    }
}