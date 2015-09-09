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

using System.Collections.Generic;

namespace QwLua.Handler
{
    internal sealed class HandlerCenter
    {
        private readonly IDictionary<object, object> _cache = new Dictionary<object, object>(10);

        public void Add(object o, object handler)
        {
            if (!_cache.ContainsKey(o))
            {
                _cache.Add(o, handler);
            }
        }

        public object this[int index]
        {
            get
            {
                if (_cache.ContainsKey(index))
                    return _cache[index];
                return null;
            }
        }

        public object this[object o]
        {
            get
            {
                if (_cache.ContainsKey(o))
                    return _cache[o];
                return null;
            }
        }

        public void Remove(object index)
        {
            if (_cache.ContainsKey(index))
            {
                _cache.Remove(index);
            }
        }

        public bool IsExist(object key)
        {
            return _cache.ContainsKey(key);
        }

        public void Dispose()
        {
            _cache.Clear();
        }
    }
}