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
using System.Runtime.InteropServices;
using QwLua.Data;
using QwLua.Event;
using QwLua.Exceptions;
using QwLua.Helpers;

namespace QwLua.Implements
{
    [CLSCompliant(true)]
    internal sealed class LuaRuntime : ILuaRuntime
    {
        private readonly RuntimeDisposer _disposer;
        private readonly object _mark;
        private readonly static ScriptFunctionHandler ShowError = ThrowLuaError;
        private readonly HookManager _hookManager;

        private const string ChunkName = "LuaRuntime";
        private readonly ScriptState _luaState;
        private readonly FunctionManager _funcMgr;
        private readonly ClassManager _classMgr;
        private readonly ObjectManager _objectManager;

        public string Script { get; private set; }

        public ScriptState State
        {
            get { return _luaState; }
        }

        public RuntimeDisposer Disposer
        {
            get { return _disposer; }
        }

        internal ObjectManager ObjectMgr
        {
            get { return _objectManager; }
        }

        #region IDisposable

        ~LuaRuntime()
        {
            Dispose(false);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                Disposer.Activate();
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }

        #endregion

        public LuaRuntime()
            : this(new object())
        {
        }

        public LuaRuntime(object mark)
        {
            _mark = mark;
            var that = this;
            Script = string.Empty;
            _disposer = new RuntimeDisposer(() =>
            {
                Script = string.Empty;
                that._hookManager.RemoveHook();
                HookManagerPool.Remove(that.State);
                LuaCore.Close(that.State);
                LuaRuntimePool.Remove(that.State);
                ObjectMgr.Dispose();
                ClassMgr.Dispose();
            });
            _luaState = LuaCore.Initialze();
            _funcMgr = new FunctionManager(_luaState);
            _classMgr = new ClassManager(_luaState);
            _objectManager = new ObjectManager();

            LuaCore.Atpanic(State, ShowError);
            LuaRuntimePool.Add(_luaState, this);

            _hookManager = new HookManager(that);
            _hookManager.SetHook(EventMasks.All, 0);

            HookManagerPool.SetGlobalHookCallback();
            HookManagerPool.Add(_luaState, _hookManager);
        }

        [AllowReversePInvokeCalls]
        private static int ThrowLuaError(ScriptState luaState)
        {
            string reason = string.Format("Unprotected error in call to Lua API ({0})",
                LuaCore.ToString(luaState, -1));
            throw new LuaException(reason);
        }

        public object Mark
        {
            get { return _mark; }
        }

        public bool Disposed
        {
            get { return Disposer.Disposed; }
        }

        public IHookManager HookMgr
        {
            get { return _hookManager; }
        }

        public FunctionManager FuncMgr
        {
            get { return _funcMgr; }
        }

        public ClassManager ClassMgr
        {
            get { return _classMgr; }
        }

        #region Impl

        public IList<object> RunScript(string script)
        {
            if (Disposed)
                return null;
            Script = script;
            var luaState = State;
            int oldTop = LuaCore.GetTop(luaState);
            if (LuaCore.LoadBuffer(luaState, script, 0, ChunkName) == 0)
            {
                if (LuaCore.Call(luaState, 0, -1, 0) == 0)
                {
                    return ObjectMgr.PopResults(luaState, oldTop);
                }
                ThrowExceptionFromError(luaState, oldTop);
            }
            else
            {
                ThrowExceptionFromError(luaState, oldTop);
            }
            return null;
        }

        public IActuator RunScriptAsync(string script)
        {
            return RunScriptAsync(script, null);
        }

        public IActuator RunScriptAsync(string script, Action<IList<object>> callback)
        {
            Script = script;
            Disposer.Add();
            var actuator = new RuntimeActuator();
            var arg = new RuntimeActuator.Argument(this, script, callback);
            actuator.Start(arg);
            return actuator;
        }

        public ITable NewTable(string fullPath)
        {
            ScriptHelper.NewTable(State, fullPath);
            return GetTable(fullPath);
        }

        public ITable GetTable(string fullPath)
        {
            return this[fullPath] as Table;
        }

        public void ThrowError(Exception ex)
        {
            ThrowError(State, ex);
        }

        public void ThrowError(string message)
        {
            ThrowError(State, message);
        }

        public void ThrowError(string format, params object[] args)
        {
            ThrowError(string.Format(format, args));
        }

        public object this[string fullPath]
        {
            get
            {
                if (Disposed) return null;
                return ScriptHelper.SearchLuaObject(State, fullPath);
            }
            set
            {
                if (Disposed) return;
                var o = value;
                ScriptHelper.SearchSetLuaObject(State, fullPath, o);
                if (o == null)
                {
                    Remove(fullPath);
                }
            }
        }

        public void Remove(string fullPath)
        {
            if (Disposed) return;
            ScriptHelper.SearchSetLuaObject(State, fullPath, null);
        }

        public void TypeRegister(Type type)
        {
            if (Disposed) return;
            ClassMgr.Register(type);
        }

        public void TypeRegister(Type type, string fullPath)
        {
            if (Disposed) return;
            ClassMgr.Register(type, fullPath);
        }

        public void ObjectRegister(object o)
        {
            if (Disposed) return;
            ClassMgr.ObjectRegister(o);
        }

        public void ObjectRegister(object o, string fullPath)
        {
            if (Disposed) return;
            ClassMgr.ObjectRegister(fullPath, o);
        }

        public void AssemblyRegister(Assembly assembly)
        {
            if (Disposed) return;
            ClassMgr.AssemblyRegister(assembly);
        }

        public IScriptFunction GetFunc(string funcName)
        {
            if (Disposed) return null;
            return FuncMgr.GetFunction(funcName);
        }

        public IList<object> Invoke(string funcName, params object[] args)
        {
            if (Disposed) return null;
            return FuncMgr.Invoke(funcName, args);
        }

        public IScriptFunction FuncRegister(string fullPath, Func<IList<object>, object> func)
        {
            if (Disposed) return null;
            return FuncMgr.Register(fullPath, func);
        }

        #endregion

        internal void ThrowExceptionFromError(ScriptState luaState, int oldTop)
        {
            object err = DataHelper.GetObject(luaState, -1);
            LuaCore.SetTop(luaState, oldTop);
            var luaEx = err as LuaSourcetException;

            if (luaEx != null)
                throw luaEx;
            if (err == null)
                err = "Unknown Lua Error";
            throw new LuaSourcetException(err.ToString(), string.Empty);
        }

        internal void ThrowError(ScriptState luaState, object e)
        {
            if (Disposed) return;
            // We use this to remove anything pushed by luaL_where
            int oldTop = LuaCore.GetTop(luaState);

            // Stack frame #1 is our C# wrapper, so not very interesting to the user
            // Stack frame #2 must be the lua code that called us, so that's what we want to use
            LuaCore.Where(luaState, 1);
            var curlev = ObjectMgr.PopResults(luaState, oldTop);

            // Determine the position in the script where the exception was triggered
            string errLocation = string.Empty;

            if (curlev.Count > 0)
                errLocation = curlev[0].ToString();

            var message = e as string;
            if (message != null)
            {
                e = new LuaSourcetException(message, errLocation);
            }
            else
            {
                var ex = e as Exception;
                if (ex != null)
                {
                    e = new LuaSourcetException(ex, errLocation);
                }
            }
            ObjectMgr.PushObject(luaState, e);
            LuaCore.Error(luaState);
        }
    }
}