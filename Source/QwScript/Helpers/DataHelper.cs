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
using QwLua.Exceptions;
using QwLua.Implements;
using QwLua.Metatable;

namespace QwLua.Helpers
{
    internal sealed class DataHelper
    {
        #region Mapping

        private static readonly IDictionary<Type, ScriptTypes> TypeMappings = new Dictionary<Type, ScriptTypes>
        {
            {typeof (string), ScriptTypes.String},
            {typeof (sbyte), ScriptTypes.Number},
            {typeof (byte), ScriptTypes.Number},
            {typeof (short), ScriptTypes.Number},
            {typeof (ushort), ScriptTypes.Number},
            {typeof (int), ScriptTypes.Number},
            {typeof (uint), ScriptTypes.Number},
            {typeof (long), ScriptTypes.Number},
            {typeof (float), ScriptTypes.Number},
            {typeof (ulong), ScriptTypes.Number},
            {typeof (decimal), ScriptTypes.Number},
            {typeof (double), ScriptTypes.Number},
            {typeof (char), ScriptTypes.Number},
            {typeof (bool), ScriptTypes.Boolean},
            {typeof (LuaException), ScriptTypes.UserData}
        };
        #endregion

        public static void PushObject(ScriptState luaState, object o)
        {
            if (o == null)
            {
                LuaCore.PushNull(luaState);
                return;
            }
            var s = o as string;
            if (s != null)
            {
                var str = s;
                LuaCore.PushString(luaState, str);
                return;
            }
            var table = o as Table;
            if (table != null)
            {
                table.Push(luaState);
                return;
            }

            var runtime = LuaRuntimePool.GetRuntime(luaState);
            var objectMgr = runtime.ObjectMgr;

            var func = o as ScriptFunctionHandler;
            if (func != null)
            {
                objectMgr.PushObjectEx(luaState, func, FunctionMetatable.GlobalName);
                return;
            }
            var type = o as Type;
            if (type != null)
            {
                objectMgr.PushObjectEx(luaState, type, ObjectMetatable.GlobalName);
                return;
            }
            if (o is sbyte || o is byte || o is short || o is ushort ||
                    o is int || o is uint || o is long || o is float ||
                    o is ulong || o is decimal || o is double)
            {
                double d = Convert.ToDouble(o);
                LuaCore.PushNumber(luaState, d);
            }
            else if (o is char)
            {
                double d = (char)o;
                LuaCore.PushNumber(luaState, d);
            }
            else if (o is bool)
            {
                var b = (bool)o;
                LuaCore.PushBool(luaState, b);
            }
            else
            {
                var funcResult = o as IFunctionResult;
                if (funcResult != null)
                {
                    for (int i = 0; i < funcResult.ArgumentLength; i++)
                    {
                        PushObject(luaState, funcResult.Data[i]);
                    }
                    return;
                }
                objectMgr.PushObjectEx(luaState, o, ObjectMetatable.GlobalName);
            }
        }

        public static object GetNetObject(ScriptState luaState, int index)
        {
            var runtime = LuaRuntimePool.GetRuntime(luaState);
            return runtime.ObjectMgr.GetObject(luaState, index);
        }

        public static T GetNetObject<T>(ScriptState luaState, int index)
        {
            var runtime = LuaRuntimePool.GetRuntime(luaState);
            return runtime.ObjectMgr.GetObject<T>(luaState, index);
        }

        public static object GetObject(ScriptState luaState, int index)
        {
            var luaType = LuaCore.GetType(luaState, index);
            return GetObject(luaType, luaState, index);
        }

        public static object GetObject(ScriptTypes luaType, ScriptState luaState, int index)
        {
            switch (luaType)
            {
                case ScriptTypes.Boolean:
                    return LuaCore.ToBool(luaState, index);
                case ScriptTypes.Number:
                    return LuaCore.ToNumber(luaState, index);
                case ScriptTypes.String:
                    return LuaCore.ToString(luaState, index);
                case ScriptTypes.Table:
                    return new Table(luaState, index);
                case ScriptTypes.Function:
                    return new ScriptFunction(luaState, index);
                case ScriptTypes.UserData:
                    var runtime = LuaRuntimePool.GetRuntime(luaState);
                    return runtime.ObjectMgr.GetObject(luaState, index);
            }
            return null;
        }

        public static object GetObject(Type argType, ScriptState luaState, int index)
        {
            if (TypeMappings.ContainsKey(argType))
            {
                var luaType = TypeMappings[argType];
                return GetObject(luaType, luaState, index);
            }
            return null;
        }
    }
}