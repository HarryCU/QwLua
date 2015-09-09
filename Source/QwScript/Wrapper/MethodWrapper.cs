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
using System.Linq;
using QwLua.Data;
using QwLua.Helpers;
using QwLua.Implements;

namespace QwLua.Wrapper
{
    internal sealed class MethodWrapper
    {
        private readonly MethodFinder _finder;
        private readonly ScriptFunctionHandler _functionHandler;

        public MethodWrapper(MethodFinder finder)
        {
            _finder = finder;
            _functionHandler = Call;
        }

        public ScriptFunctionHandler Invoker
        {
            get { return _functionHandler; }
        }

        public MethodFinder Finder
        {
            get { return _finder; }
        }

        private int Call(ScriptState luaState)
        {
            var finder = Finder;
            var runtime = LuaRuntimePool.GetRuntime(luaState);

            object instance = null;
            int argIndex = 1;

            if (!finder.IsStatic)
            {
                instance = DataHelper.GetObject(luaState, 1);
                argIndex = 2;
            }
            var n = LuaCore.GetTop(luaState);
            IList<object> args = new List<object>(argIndex);
            IList<object> argTypes = new List<object>(argIndex);
            for (; argIndex <= n; argIndex++)
            {
                var arg = DataHelper.GetObject(luaState, argIndex);
                args.Add(arg);
                argTypes.Add(arg == null ? typeof(object) : arg.GetType());
            }
            var m = finder.Search(argTypes);
            if (m == null)
            {
                string memberKey = finder.CreateKey(argTypes);
                runtime.ThrowError("未能找到" + memberKey + "的方法!");
            }
            argTypes.Clear();
            object result = null;
            try
            {
                if (m != null)
                    result = m.Invoke(instance, args.ToArray());
            }
            catch (Exception ex)
            {
                runtime.ThrowError(ex);
            }
            finally
            {
                args.Clear();
            }
            DataHelper.PushObject(luaState, result);
            return 1;
        }
    }
}
