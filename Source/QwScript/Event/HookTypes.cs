using System;

namespace QwLua.Event
{
    [Flags]
    public enum HookTypes
    {
        Call = 0,
        Ret = 1,
        Line = 2,
        Count = 3,
        Tailret = 4
    }
}
