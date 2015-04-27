using System;
using QwLua.Event;

namespace QwLua
{
    public interface IHookManager
    {
        event EventHandler<HookEventArgs> Hook;
        event EventHandler<ErrorEventArgs> Error;
    }
}