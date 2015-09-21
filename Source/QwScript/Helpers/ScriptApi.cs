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

namespace QwLua.Helpers
{
    internal static class ScriptApi
    {
        private const string LuaImportDllName = @"E:\ZOE\QwLua\out\Debug\lua.dll";

        [DllImport(LuaImportDllName, EntryPoint = "ldotNet_setDotNetHook", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SetDotNetHook(IntPtr ptr);

        [DllImport(LuaImportDllName, EntryPoint = "ldotNet_gc", CallingConvention = CallingConvention.Cdecl)]
        public static extern void GC(IntPtr ptr, int what, int data);

        [DllImport(LuaImportDllName, EntryPoint = "ldotNet_initlizeEngine", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr Initialze();

        [DllImport(LuaImportDllName, EntryPoint = "ldotNet_closeEngine", CallingConvention = CallingConvention.Cdecl)]
        public static extern void Close(IntPtr luaState);

        [DllImport(LuaImportDllName, EntryPoint = "ldotNet_pushdotNetMethod", CallingConvention = CallingConvention.Cdecl)]
        public static extern void PushInvokeMethod(IntPtr luaState, IntPtr call);

        [DllImport(LuaImportDllName, EntryPoint = "ldotNet_pushFunction", CallingConvention = CallingConvention.Cdecl)]
        public static extern void PushFunction(IntPtr luaState, IntPtr call);


        [DllImport(LuaImportDllName, EntryPoint = "ldotNet_runScript", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern int RunScript(IntPtr luaState, string script);

        [DllImport(LuaImportDllName, EntryPoint = "ldotNet_runFile", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern int RunFile(IntPtr luaState, string fileName);

        [DllImport(LuaImportDllName, EntryPoint = "ldotNet_pushError", CallingConvention = CallingConvention.Cdecl)]
        public static extern int Error(IntPtr luaState);


        [DllImport(LuaImportDllName, EntryPoint = "ldotNet_getLuaType", CallingConvention = CallingConvention.Cdecl)]
        public static extern int GetType(IntPtr luaState, int index);


        [DllImport(LuaImportDllName, EntryPoint = "ldotNet_pushNull", CallingConvention = CallingConvention.Cdecl)]
        public static extern void PushNull(IntPtr luaState);

        [DllImport(LuaImportDllName, EntryPoint = "ldotNet_pushNumber", CallingConvention = CallingConvention.Cdecl)]
        public static extern void PushNumber(IntPtr luaState, double d);

        [DllImport(LuaImportDllName, EntryPoint = "ldotNet_pushBool", CallingConvention = CallingConvention.Cdecl)]
        public static extern void PushBool(IntPtr luaState, int b);

        [DllImport(LuaImportDllName, EntryPoint = "ldotNet_pushString", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern void PushString(IntPtr luaState, string str);

        [DllImport(LuaImportDllName, EntryPoint = "ldotNet_pushInt", CallingConvention = CallingConvention.Cdecl)]
        public static extern void PushInt(IntPtr luaState, int i);


        [DllImport(LuaImportDllName, EntryPoint = "ldotNet_toNumber", CallingConvention = CallingConvention.Cdecl)]
        public static extern double ToNumber(IntPtr luaState, int index);

        [DllImport(LuaImportDllName, EntryPoint = "ldotNet_toInt", CallingConvention = CallingConvention.Cdecl)]
        public static extern int ToInt(IntPtr luaState, int index);

        [DllImport(LuaImportDllName, EntryPoint = "ldotNet_toString", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr ToString(IntPtr luaState, int index);

        [DllImport(LuaImportDllName, EntryPoint = "ldotNet_toBool", CallingConvention = CallingConvention.Cdecl)]
        public static extern int ToBool(IntPtr luaState, int index);


        [DllImport(LuaImportDllName, EntryPoint = "ldotNet_atpanic", CallingConvention = CallingConvention.Cdecl)]
        public static extern void Atpanic(IntPtr luaState, IntPtr call);

        [DllImport(LuaImportDllName, EntryPoint = "ldotNet_registerGlobalMethod", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern void RegisterMethod(IntPtr luaState, string methodName, IntPtr call);

        [DllImport(LuaImportDllName, EntryPoint = "ldotNet_getRef", CallingConvention = CallingConvention.Cdecl)]
        public static extern int GetRef(IntPtr luaState, int index);

        [DllImport(LuaImportDllName, EntryPoint = "ldotNet_getGlobalRef", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern int GetGlobalRef(IntPtr luaState, string globalName, int luaType);

        [DllImport(LuaImportDllName, EntryPoint = "ldotNet_loopTable", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern void ForeachTable(IntPtr luaState, int index, IntPtr call);

        [DllImport(LuaImportDllName, EntryPoint = "ldotNet_setTableValue", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SetTableValue(IntPtr luaState, int index, IntPtr call);

        [DllImport(LuaImportDllName, EntryPoint = "ldotNet_createTable", CallingConvention = CallingConvention.Cdecl)]
        public static extern void CreateTable(IntPtr luaState);

        [DllImport(LuaImportDllName, EntryPoint = "ldotNet_getTable", CallingConvention = CallingConvention.Cdecl)]
        public static extern void GetTable(IntPtr luaState, int index);

        [DllImport(LuaImportDllName, EntryPoint = "ldotNet_setTable", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SetTable(IntPtr luaState, int index);

        [DllImport(LuaImportDllName, EntryPoint = "ldotNet_setGlobalTable", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SetGlobalTable(IntPtr luaState);

        [DllImport(LuaImportDllName, EntryPoint = "ldotNet_newMetatable", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern void NewMetatable(IntPtr luaState, string name);

        [DllImport(LuaImportDllName, EntryPoint = "ldotNet_getMetatable", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern void GetMetatable(IntPtr luaState, string name);

        [DllImport(LuaImportDllName, EntryPoint = "ldotNet_setMetatable", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SetMetatable(IntPtr luaState, int index);

        [DllImport(LuaImportDllName, EntryPoint = "ldotNet_setHook", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SetHook(IntPtr luaState, int mask, int count);

        [DllImport(LuaImportDllName, EntryPoint = "ldotNet_getHookCount", CallingConvention = CallingConvention.Cdecl)]
        public static extern int GetHookCount(IntPtr luaState);

        [DllImport(LuaImportDllName, EntryPoint = "ldotNet_getLocal", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr GetLocal(IntPtr luaState, IntPtr ar, int n);

        [DllImport(LuaImportDllName, EntryPoint = "ldotNet_setLocal", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SetLocal(IntPtr luaState, IntPtr ar, int n);

        [DllImport(LuaImportDllName, EntryPoint = "ldotNet_getHookMask", CallingConvention = CallingConvention.Cdecl)]
        public static extern int GetHookMask(IntPtr luaState);

        [DllImport(LuaImportDllName, EntryPoint = "ldotNet_globalRawGet", CallingConvention = CallingConvention.Cdecl)]
        public static extern void GlobalRawGet(IntPtr luaState, int index);

        [DllImport(LuaImportDllName, EntryPoint = "ldotNet_newUserData", CallingConvention = CallingConvention.Cdecl)]
        public static extern void NewUserData(IntPtr luaState, int value);

        [DllImport(LuaImportDllName, EntryPoint = "ldotNet_toUserData", CallingConvention = CallingConvention.Cdecl)]
        public static extern int ToUserData(IntPtr luaState, int index);

        [DllImport(LuaImportDllName, EntryPoint = "ldotNet_checkUserData", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern int CheckUserData(IntPtr luaState, int index, string name);

        [DllImport(LuaImportDllName, EntryPoint = "ldotNet_pushValue", CallingConvention = CallingConvention.Cdecl)]
        public static extern void PushValue(IntPtr luaState, int index);

        [DllImport(LuaImportDllName, EntryPoint = "ldotNet_removeValue", CallingConvention = CallingConvention.Cdecl)]
        public static extern void RemoveValue(IntPtr luaState, int index);


        [DllImport(LuaImportDllName, EntryPoint = "ldotNet_rawsSet", CallingConvention = CallingConvention.Cdecl)]
        public static extern void RawSet(IntPtr luaState, int index);

        [DllImport(LuaImportDllName, EntryPoint = "ldotNet_rawsGet", CallingConvention = CallingConvention.Cdecl)]
        public static extern void RawGet(IntPtr luaState, int index);

        [DllImport(LuaImportDllName, EntryPoint = "ldotNet_loadBuffer", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern int LoadBuffer(IntPtr luaState, string buff, uint size, string name);


        [DllImport(LuaImportDllName, EntryPoint = "ldotNet_getTop", CallingConvention = CallingConvention.Cdecl)]
        public static extern int GetTop(IntPtr luaState);

        [DllImport(LuaImportDllName, EntryPoint = "ldotNet_setTop", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SetTop(IntPtr luaState, int index);

        [DllImport(LuaImportDllName, EntryPoint = "ldotNet_call", CallingConvention = CallingConvention.Cdecl)]
        public static extern int Call(IntPtr luaState, int nargs, int nresults, int errfunc);


        [DllImport(LuaImportDllName, EntryPoint = "ldotNet_getGlobal", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern void GetGlobal(IntPtr luaState, string globalName);

        [DllImport(LuaImportDllName, EntryPoint = "ldotNet_setGlobal", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern void SetGlobal(IntPtr luaState, string globalName);

        [DllImport(LuaImportDllName, EntryPoint = "ldotNet_rawSetI", CallingConvention = CallingConvention.Cdecl)]
        public static extern int RawSetIndex(IntPtr luaState, int tableIndex, int index);

        [DllImport(LuaImportDllName, EntryPoint = "ldotNet_rawGetI", CallingConvention = CallingConvention.Cdecl)]
        public static extern int RawGetIndex(IntPtr luaState, int tableIndex, int index);

        [DllImport(LuaImportDllName, EntryPoint = "ldotNet_where", CallingConvention = CallingConvention.Cdecl)]
        public static extern void Where(IntPtr luaState, int level);

        [DllImport(LuaImportDllName, EntryPoint = "ldotNet_rawLength", CallingConvention = CallingConvention.Cdecl)]
        public static extern uint RawLength(IntPtr luaState, int index);

        [DllImport(LuaImportDllName, EntryPoint = "ldotNet_rawIsLuaType", CallingConvention = CallingConvention.Cdecl)]
        public static extern int RawIsLuaType(IntPtr luaState, int luaType, int index);

        [DllImport(LuaImportDllName, EntryPoint = "ldotNet_getInfo", CallingConvention = CallingConvention.Cdecl)]
        public static extern int GetHookInfo(IntPtr luaState, ref HookInfo info);

        [DllImport(LuaImportDllName, EntryPoint = "ldotNet_getStack", CallingConvention = CallingConvention.Cdecl)]
        public static extern int GetStack(IntPtr luaState, int level, ref HookInfo info);

        [DllImport(LuaImportDllName, EntryPoint = "ldotNet_pop", CallingConvention = CallingConvention.Cdecl)]
        public static extern void Pop(IntPtr luaState, int index);

        [DllImport(LuaImportDllName, EntryPoint = "ldotNet_freeDebug", CallingConvention = CallingConvention.Cdecl)]
        public static extern void FreeDebug(HookInfo info);
    }
}