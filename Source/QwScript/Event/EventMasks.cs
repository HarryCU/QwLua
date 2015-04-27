using System;

namespace QwLua.Event
{
    [Flags]
    public enum EventMasks
    {
        Call = (1 << HookTypes.Call),
        Ret = (1 << HookTypes.Ret),
        Line = (1 << HookTypes.Line),
        Count = (1 << HookTypes.Count),
        All = Call | Ret | Line | Count,
        None = 0
    }
}
