using System;
using System.Collections.Generic;
using System.Threading;
using QwLua.Event;
using QwLua.Utils;

namespace QwLua.Implements
{
    internal sealed class RuntimeActuator : BaseActuator
    {
        public class Argument : BaseArgument
        {
            private readonly Action<IList<object>> _callback;

            public Argument(LuaRuntime runtime, string script, Action<IList<object>> callback)
                : base(script)
            {
                Runtime = runtime;
                _callback = callback;
            }

            public LuaRuntime Runtime { get; private set; }

            public void SendResult(IList<object> result)
            {
                Runtime.Disposer.Subtract();
                if (_callback != null)
                {
                    _callback(result);
                }
            }
        }

        protected override void ThreadProc(object d)
        {
            var arg = d as Argument;
            if (arg == null) return;

            var runtime = arg.Runtime;

            runtime.HookMgr.Hook += Sleep;
            var result = runtime.RunScript(arg.Script);
            runtime.HookMgr.Hook -= Sleep;
            arg.SendResult(result);
        }

        private void Sleep(object sender, HookEventArgs e)
        {
            while (Paused)
            {
                Thread.Sleep(50);
            }
        }
    }
}
