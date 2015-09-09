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
using System.ComponentModel;

namespace QwLua.Core
{
    public sealed class EventManager : Disposer
    {
        private EventManager() { }

        private readonly EventHandlerList _eventHandlers = new EventHandlerList();

        public void Add(object key, Delegate @delegate)
        {
            _eventHandlers.AddHandler(key, @delegate);
        }

        public void Remove(object key, Delegate @delegate)
        {
            _eventHandlers.RemoveHandler(key, @delegate);
        }

        public T GetHandler<T>(object key)
        {
            return (T)(object)_eventHandlers[key];
        }

        public EventHandler<T> GetEventHandler<T>(object key)
        {
            return _eventHandlers[key] as EventHandler<T>;
        }

        public static EventManager Create()
        {
            return new EventManager();
        }

        protected override void Release()
        {
            _eventHandlers.Dispose();
        }
    }
}