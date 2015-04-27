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

        IList<object> RunScript(string script);

        IActuator RunScriptAsync(string script);
        IActuator RunScriptAsync(string script, Action<IList<object>> callback);

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
        IList<object> Invoke(string funcName, params object[] args);

        IScriptFunction FuncRegister(string fullPath, Func<IList<object>, object> func);
    }
}