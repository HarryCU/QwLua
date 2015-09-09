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
using QwLua.Implements;
using QwLua.Metatable;

namespace QwLua.Helpers
{
    internal static class ScriptHelper
    {
        public static string[] GetSearchPaths(string fullPath)
        {
            return fullPath.Split('.');
        }

        public static object SearchLuaObject(ScriptState luaState, string fullPath)
        {
            var oldTop = LuaCore.GetTop(luaState);
            var paths = GetSearchPaths(fullPath);
            LuaCore.GetGlobal(luaState, paths[0]);
            var result = DataHelper.GetObject(luaState, -1);
            if (paths.Length > 1)
            {
                for (int i = 1; i < paths.Length; i++)
                {
                    LuaCore.PushString(luaState, paths[i]);
                    LuaCore.GetTable(luaState, -2);
                    result = DataHelper.GetObject(luaState, -1);
                    if (result == null)
                        break;
                }
            }
            LuaCore.SetTop(luaState, oldTop);
            return result;
        }

        public static void SearchSetLuaObject(ScriptState luaState, string fullPath, object o)
        {
            var oldTop = LuaCore.GetTop(luaState);
            var paths = GetSearchPaths(fullPath);
            if (paths.Length == 1)
            {
                DataHelper.PushObject(luaState, o);
                LuaCore.SetGlobal(luaState, paths[0]);
            }
            else
            {
                LuaCore.GetGlobal(luaState, paths[0]);
                for (int i = 1; i < paths.Length - 1; i++)
                {
                    LuaCore.PushString(luaState, paths[i]);
                    LuaCore.GetTable(luaState, -2);
                }
                LuaCore.PushString(luaState, paths[paths.Length - 1]);
                DataHelper.PushObject(luaState, o);
                LuaCore.SetTable(luaState, -3);
            }
            LuaCore.SetTop(luaState, oldTop);
        }

        public static void SearchSetLuaObjectEx(ScriptState luaState, string fullPath, object o, string metatableName)
        {
            var oldTop = LuaCore.GetTop(luaState);
            var runtime = LuaRuntimePool.GetRuntime(luaState);
            var paths = GetSearchPaths(fullPath);
            if (paths.Length == 1)
            {
                runtime.ObjectMgr.PushObjectEx(luaState, o, metatableName);
                LuaCore.SetGlobal(luaState, paths[0]);
            }
            else
            {
                LuaCore.GetGlobal(luaState, paths[0]);
                for (int i = 1; i < paths.Length - 1; i++)
                {
                    LuaCore.PushString(luaState, paths[i]);
                    LuaCore.GetTable(luaState, -2);
                }
                LuaCore.PushString(luaState, paths[paths.Length - 1]);
                runtime.ObjectMgr.PushObjectEx(luaState, o, metatableName);
                LuaCore.SetTable(luaState, -3);
            }
            LuaCore.SetTop(luaState, oldTop);
        }

        public static string GetMetatableNameByType(Type type)
        {
            if (type.IsEnum)
            {
                return EnumMetatable.GlobalName;
            }
            return ObjectMetatable.GlobalName;
        }

        public static string GenerateKey(Type type, string memberName)
        {
            return type.FullName + memberName;
        }

        private static bool TableNotFound(LuaRuntime runtime, ScriptState luaState, int index, string tableName)
        {
            if (LuaCore.GetType(luaState, index) != ScriptTypes.Table)
            {
                runtime.ThrowError("未能找到全局Table=>{0}", tableName);
                return false;
            }
            return true;
        }

        public static bool NewTable(ScriptState luaState, string fullPath)
        {
            var runtime = LuaRuntimePool.GetRuntime(luaState);
            var oldTop = LuaCore.GetTop(luaState);
            var paths = GetSearchPaths(fullPath);
            var tableName = paths[0];
            if (paths.Length > 1)
            {
                LuaCore.GetGlobal(luaState, paths[0]);
                if (!TableNotFound(runtime, luaState, -1, paths[0]))
                    return false;
                for (int i = 1; i < paths.Length - 1; i++)
                {
                    LuaCore.PushString(luaState, paths[i]);
                    LuaCore.GetTable(luaState, -2);
                    if (!TableNotFound(runtime, luaState, -1, paths[i]))
                        return false;
                }
                tableName = paths[paths.Length - 1];
            }
            LuaCore.CreateTable(luaState);
            LuaCore.SetGlobal(luaState, tableName);
            LuaCore.SetTop(luaState, oldTop);
            return true;
        }
    }
}
