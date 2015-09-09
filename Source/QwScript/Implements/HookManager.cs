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
using QwLua.Core;
using QwLua.Event;

namespace QwLua.Implements
{
    internal sealed class HookManager : IHookManager
    {
        private readonly EventManager _eventManager = EventManager.Create();
        private readonly object _hookKey = new object();
        private readonly object _errorKey = new object();

        private readonly ILuaRuntime _runtime;
        public bool Disposed
        {
            get { return _runtime.Disposed; }
        }

        public ILuaRuntime Runtime
        {
            get { return _runtime; }
        }

        public event EventHandler<HookEventArgs> Hook
        {
            add
            {
                _eventManager.Add(_hookKey, value);
            }
            remove
            {
                _eventManager.Remove(_hookKey, value);
            }
        }

        public event EventHandler<ErrorEventArgs> Error
        {
            add
            {
                _eventManager.Add(_errorKey, value);
            }
            remove
            {
                _eventManager.Remove(_errorKey, value);
            }
        }

        public HookManager(ILuaRuntime runtime)
        {
            _runtime = runtime;
        }

        public int SetHook(EventMasks mask, int count)
        {
            if (Disposed) return -1;
            return LuaCore.SetHook(Runtime.State, mask, count);
        }

        public int RemoveHook()
        {
            if (Disposed) return -1;
            return LuaCore.SetHook(Runtime.State, 0, 0);
        }

        public EventMasks GetHookMask()
        {
            if (Disposed) return EventMasks.None;
            return LuaCore.GetHookMask(Runtime.State);
        }

        public int GetHookCount()
        {
            if (Disposed) return -1;
            return LuaCore.GetHookCount(Runtime.State);
        }

        public void TriggerHook(HookEventArgs e)
        {
            var handler = _eventManager.GetHandler<EventHandler<HookEventArgs>>(_hookKey);
            if (handler != null)
                handler(this, e);
        }

        public void TriggerError(ErrorEventArgs e)
        {
            var handler = _eventManager.GetHandler<EventHandler<ErrorEventArgs>>(_errorKey);
            if (handler != null)
                handler(this, e);
        }
    }
}
