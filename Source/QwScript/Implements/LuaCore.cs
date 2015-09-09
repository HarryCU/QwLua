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
using System.Runtime.InteropServices;
using QwLua.Data;
using QwLua.Event;
using QwLua.Helpers;

namespace QwLua.Implements
{
    internal static class LuaCore
    {
        private static IntPtr ConvertDelegateToIntPtr(Delegate @delegate)
        {
            return Marshal.GetFunctionPointerForDelegate(@delegate);
        }

        private static ScriptFunctionHandler CreateNativeFunction(Action<ScriptState> action)
        {
            return luaState =>
            {
                if (action != null)
                {
                    action(luaState);
                }
                return 0;
            };
        }

        public static void GC(ScriptState luaState, GCOptions gcOptions, int data)
        {
            ScriptApi.GC(luaState, (int)gcOptions, data);
        }

        public static void SetDotNetHook(DotNetHookHandler hook)
        {
            var ptr = ConvertDelegateToIntPtr(hook);
            ScriptApi.SetDotNetHook(ptr);
        }

        public static ScriptState Initialze()
        {
            return ScriptApi.Initialze();
        }

        public static void Close(ScriptState luaState)
        {
            ScriptApi.Close(luaState);
        }

        public static void PushInvokeMethod(ScriptState luaState, ScriptFunctionHandler call)
        {
            IntPtr ptrCall = ConvertDelegateToIntPtr(call);
            ScriptApi.PushInvokeMethod(luaState, ptrCall);
        }

        public static void PushFunction(ScriptState luaState, ScriptFunctionHandler call)
        {
            IntPtr ptrCall = ConvertDelegateToIntPtr(call);
            ScriptApi.PushFunction(luaState, ptrCall);
        }

        public static int RunScript(ScriptState luaState, string script)
        {
            return ScriptApi.RunScript(luaState, script);
        }

        public static int RunFile(ScriptState luaState, string fileName)
        {
            return ScriptApi.RunFile(luaState, fileName);
        }

        public static int Error(ScriptState luaState)
        {
            return ScriptApi.Error(luaState);
        }

        private static int ConvertParmenterIndex(int index)
        {
            return index;
        }

        public static ScriptTypes GetType(ScriptState luaState, int index)
        {
            return (ScriptTypes)ScriptApi.GetType(luaState, ConvertParmenterIndex(index));
        }

        public static void PushNull(ScriptState luaState)
        {
            ScriptApi.PushNull(luaState);
        }

        public static void PushNumber(ScriptState luaState, double d)
        {
            ScriptApi.PushNumber(luaState, d);
        }

        public static void PushBool(ScriptState luaState, bool b)
        {
            ScriptApi.PushBool(luaState, b ? 1 : 0);
        }

        public static void PushString(ScriptState luaState, string str)
        {
            ScriptApi.PushString(luaState, str);
        }

        public static void PushInt(ScriptState luaState, int i)
        {
            ScriptApi.PushInt(luaState, i);
        }

        public static double ToNumber(ScriptState luaState, int index)
        {
            return ScriptApi.ToNumber(luaState, ConvertParmenterIndex(index));
        }

        public static int ToInt(ScriptState luaState, int index)
        {
            return ScriptApi.ToInt(luaState, ConvertParmenterIndex(index));
        }

        public static string ToString(ScriptState luaState, int index)
        {
            CharPtr ptrString = ScriptApi.ToString(luaState, ConvertParmenterIndex(index));
            return ptrString.ToString();
        }

        public static bool ToBool(ScriptState luaState, int index)
        {
            var result = ScriptApi.ToBool(luaState, ConvertParmenterIndex(index));
            return result == 1;
        }

        public static void Atpanic(ScriptState luaState, ScriptFunctionHandler fn)
        {
            var ptr = ConvertDelegateToIntPtr(fn);
            ScriptApi.Atpanic(luaState, ptr);
        }

        public static void RegisterMethod(ScriptState luaState, string methodName, ScriptFunctionHandler fn)
        {
            var ptr = ConvertDelegateToIntPtr(fn);
            ScriptApi.RegisterMethod(luaState, methodName, ptr);
        }

        public static void RegisterAction(ScriptState luaState, string methodName, Action<ScriptState> action)
        {
            RegisterMethod(luaState, methodName, CreateNativeFunction(action));
        }

        public static int GetRef(ScriptState luaState, int index)
        {
            return ScriptApi.GetRef(luaState, index);
        }

        public static int GetGlobalRef(ScriptState luaState, string globalName, ScriptTypes luaType)
        {
            return ScriptApi.GetGlobalRef(luaState, globalName, (int)luaType);
        }

        public static void ForeachTable(ScriptState luaState, int index, Action<ScriptState> action)
        {
            ScriptApi.ForeachTable(luaState, index, ConvertDelegateToIntPtr(CreateNativeFunction(action)));
        }

        public static void SetTableValue(ScriptState luaState, int index, Action<ScriptState> action)
        {
            ScriptApi.SetTableValue(luaState, index, ConvertDelegateToIntPtr(CreateNativeFunction(action)));
        }

        public static void CreateTable(ScriptState luaState)
        {
            ScriptApi.CreateTable(luaState);
        }

        public static void GetTable(ScriptState luaState, int index)
        {
            ScriptApi.GetTable(luaState, index);
        }

        public static void SetTable(ScriptState luaState, int index)
        {
            ScriptApi.SetTable(luaState, index);
        }

        public static void SetGlobalTable(ScriptState luaState)
        {
            ScriptApi.SetGlobalTable(luaState);
        }

        public static void NewMetatable(ScriptState luaState, string name)
        {
            ScriptApi.NewMetatable(luaState, name);
        }

        public static void GetMetatable(ScriptState luaState, string name)
        {
            ScriptApi.GetMetatable(luaState, name);
        }

        public static void SetMetatable(ScriptState luaState, int index)
        {
            ScriptApi.SetMetatable(luaState, index);
        }

        public static int SetHook(ScriptState luaState, EventMasks mask, int count)
        {
            return ScriptApi.SetHook(luaState, (int)mask, count);
        }

        public static int GetHookCount(ScriptState luaState)
        {
            return ScriptApi.GetHookCount(luaState);
        }

        public static string GetLocal(ScriptState luaState, LuaDebug ar, int n)
        {
            CharPtr local = ScriptApi.GetLocal(luaState, ar, n);
            return local.ToString();
        }

        public static string SetLocal(ScriptState luaState, LuaDebug ar, int n)
        {
            CharPtr local = ScriptApi.SetLocal(luaState, ar, n);
            return local.ToString();
        }

        public static EventMasks GetHookMask(ScriptState luaState)
        {
            return (EventMasks)ScriptApi.GetHookMask(luaState);
        }

        public static void GlobalRawGet(ScriptState luaState, int index)
        {
            ScriptApi.GlobalRawGet(luaState, index);
        }

        [CLSCompliant(false)]
        public static void NewUserData(ScriptState luaState, int value)
        {
            ScriptApi.NewUserData(luaState, value);
        }

        public static int ToUserData(ScriptState luaState, int index)
        {
            return ScriptApi.ToUserData(luaState, index);
        }

        public static int CheckUserData(ScriptState luaState, int index, string name)
        {
            return ScriptApi.CheckUserData(luaState, index, name);
        }

        public static void PushValue(ScriptState luaState, int index)
        {
            ScriptApi.PushValue(luaState, index);
        }

        public static void RemoveValue(ScriptState luaState, int index)
        {
            ScriptApi.RemoveValue(luaState, index);
        }

        public static void RawSet(ScriptState luaState, int index)
        {
            ScriptApi.RawSet(luaState, index);
        }

        public static void RawGet(ScriptState luaState, int index)
        {
            ScriptApi.RawGet(luaState, index);
        }

        [CLSCompliant(false)]
        public static int LoadBuffer(ScriptState luaState, string buff, uint size, string name)
        {
            return ScriptApi.LoadBuffer(luaState, buff, size, name);
        }

        public static int GetTop(ScriptState luaState)
        {
            return ScriptApi.GetTop(luaState);
        }

        public static void SetTop(ScriptState luaState, int index)
        {
            ScriptApi.SetTop(luaState, index);
        }

        public static void ResetTop(ScriptState luaState, Action<ScriptState> action)
        {
            var topIndex = GetTop(luaState);
            if (action != null)
                action(luaState);
            SetTop(luaState, topIndex);
        }

        public static int Call(ScriptState luaState, int nargs, int nresults, int errfunc)
        {
            return ScriptApi.Call(luaState, nargs, nresults, errfunc);
        }

        public static void GetGlobal(ScriptState luaState, string globalName)
        {
            ScriptApi.GetGlobal(luaState, globalName);
        }

        public static void SetGlobal(ScriptState luaState, string globalName)
        {
            ScriptApi.SetGlobal(luaState, globalName);
        }

        public static void RawSetIndex(ScriptState luaState, int tableIndex, int index)
        {
            ScriptApi.RawSetIndex(luaState, tableIndex, index);
        }

        public static void RawGetIndex(ScriptState luaState, int tableIndex, int index)
        {
            ScriptApi.RawGetIndex(luaState, tableIndex, index);
        }

        public static void Where(ScriptState luaState, int level)
        {
            ScriptApi.Where(luaState, level);
        }

        public static uint RawLength(ScriptState luaState, int index)
        {
            return ScriptApi.RawLength(luaState, index);
        }

        public static bool RawIsLuaType(ScriptState luaState, ScriptTypes luaType, int index)
        {
            return ScriptApi.RawIsLuaType(luaState, (int)luaType, index) != 0;
        }

        public static HookInfo GetHookInfo(ScriptState luaState, out int result)
        {
            var info = new HookInfo();
            result = ScriptApi.GetHookInfo(luaState, ref info);
            return info;
        }

        public static HookInfo GetStack(IntPtr luaState, int level, out int result)
        {
            var info = new HookInfo();
            result = ScriptApi.GetStack(luaState, level, ref info);
            return info;
        }

        public static void Pop(ScriptState luaState, int index)
        {
            ScriptApi.Pop(luaState, index);
        }

        public static void FreeDebug(HookInfo info)
        {
            ScriptApi.FreeDebug(info);
        }
    }
}