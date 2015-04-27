using System.Runtime.InteropServices;

namespace QwLua.Data
{
    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    public delegate void DotNetHookHandler(ScriptState l, HookInfo ar);

    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    public delegate int ScriptFunctionHandler(ScriptState luaState);

    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    public delegate void DotNetFunctionHandler(ScriptState luaState);
}