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
using System.Reflection;
using QwLua.Data;

namespace QwLua
{
    public interface ILuaRuntime : IDisposable
    {
        object Mark { get; }

        string Script { get; }
        ScriptState State { get; }
        bool Disposed { get; }
        IHookManager HookMgr { get; }

        void LoadScript(string script);
        IList<object> RunScript(string script);

        ITable NewTable(string fullPath);
        ITable GetTable(string fullPath);
        void ThrowError(Exception ex);
        void ThrowError(string message);

        object this[string fullPath] { get; set; }
        void Remove(string fullPath);

        void TypeRegister(Type type);
        void TypeRegister(Type type, string fullPath);
        void ObjectRegister(object o);
        void ObjectRegister(object o, string fullPath);

        void AssemblyRegister(Assembly assembly);

        IScriptFunction GetFunc(string funcName);
        IList<object> Execute(string funcName, params object[] args);

        IScriptFunction FuncRegister(string fullPath, Func<IList<object>, object> func);
    }
}