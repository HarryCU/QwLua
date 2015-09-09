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
using System.Threading;

namespace QwLua
{
    public sealed class RuntimeDisposer
    {
        #region Locker

        private readonly object _lockKey = new object();

        private void Lock()
        {
            Monitor.Enter(_lockKey);
        }

        private void Unlock()
        {
            Monitor.Exit(_lockKey);
        }
        #endregion

        private bool _activate;
        private int _count;

        private readonly Action _disposeInvoker;

        public RuntimeDisposer(Action disposeInvoker)
        {
            _count = 0;
            _activate = false;
            Disposed = false;
            _disposeInvoker = disposeInvoker;
        }

        public bool Disposed
        {
            get;
            private set;
        }

        public bool Activated
        {
            get { return _activate; }
        }

        public void Add()
        {
            Lock();
            _count++;
            Unlock();
        }

        public void Activate()
        {
            Lock();
            _activate = true;
            Unlock();
        }

        private void Invoke()
        {
            Lock();
            if (_count <= 0 && _activate)
            {
                if (_disposeInvoker != null)
                    _disposeInvoker();
                Disposed = true;
            }
            Unlock();
        }

        public void Subtract()
        {
            Lock();
            _count--;
            Unlock();

            Invoke();
        }
    }
}
