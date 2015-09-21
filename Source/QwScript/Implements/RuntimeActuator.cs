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

            //runtime.HookMgr.Hook += Sleep;
            var result = runtime.RunScript(arg.Script);
            //runtime.HookMgr.Hook -= Sleep;
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
