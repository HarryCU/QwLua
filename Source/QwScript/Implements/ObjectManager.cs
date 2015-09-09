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
using QwLua.Data;
using QwLua.Helpers;
using QwLua.Metatable;

namespace QwLua.Implements
{
    internal sealed class ObjectManager
    {
        private int _uuid = 0;
        private readonly IDictionary<int, object> _cache = new Dictionary<int, object>(10);
        private readonly IDictionary<object, int> _mappings = new Dictionary<object, int>(10);

        public int AddObject(object o)
        {
            int index = _uuid;
            _cache.Add(index, o);
            _mappings.Add(o, index);
            _uuid++;
            return index;
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

        public int this[object o]
        {
            get
            {
                if (_mappings.ContainsKey(o))
                    return _mappings[o];
                return -1;
            }
        }

        public void Remove(ScriptState luaState, int index)
        {
            if (_cache.ContainsKey(index))
            {
                var o = _cache[index];
                _mappings.Remove(o);
                _cache.Remove(index);

                var disposer = o as IDisposable;
                if (disposer != null)
                {
                    disposer.Dispose();
                }

                // remove dotNet_objects value
                LuaCore.GetMetatable(luaState, DotNetObjectsMetatable.GlobalName);
                LuaCore.PushNull(luaState);
                LuaCore.RawSetIndex(luaState, -2, index);
            }
        }

        public object GetObject(ScriptState luaState, int index)
        {
            var uindex = LuaCore.ToUserData(luaState, index);
            return this[uindex];
        }

        public object TryGetObject(ScriptState luaState, int index, out int uindex)
        {
            uindex = LuaCore.ToUserData(luaState, index);
            return this[uindex];
        }

        public T GetObject<T>(ScriptState luaState, int index)
        {
            return (T)GetObject(luaState, index);
        }

        public int PushObject(ScriptState luaState, object o)
        {
            return PushObjectEx(luaState, o, UserDataMetatable.GlobalName);
        }

        private bool TryGetObject(ScriptState luaState, object o, out int index)
        {
            bool found = _mappings.TryGetValue(o, out index);
            if (found)
            {
                LuaCore.GetMetatable(luaState, DotNetObjectsMetatable.GlobalName);
                LuaCore.RawGetIndex(luaState, -1, index);
                var type = LuaCore.GetType(luaState, -1);
                if (type != ScriptTypes.Nil)
                {
                    LuaCore.RemoveValue(luaState, -2);
                    return true;
                }
                LuaCore.RemoveValue(luaState, -1);
                LuaCore.RemoveValue(luaState, -1);
                Remove(luaState, index);
            }
            return false;
        }

        public int PushObjectEx(ScriptState luaState, object o, string metatableName)
        {
            int index;
            if (TryGetObject(luaState, o, out index))
                return index;
            var uindex = AddObject(o);
            LuaCore.GetMetatable(luaState, metatableName);
            LuaCore.GetMetatable(luaState, DotNetObjectsMetatable.GlobalName);
            LuaCore.NewUserData(luaState, uindex);
            LuaCore.PushValue(luaState, -3);
            LuaCore.RemoveValue(luaState, -4);
            LuaCore.SetMetatable(luaState, -2);
            LuaCore.PushValue(luaState, -1);
            LuaCore.RawSetIndex(luaState, -3, uindex);
            LuaCore.RemoveValue(luaState, -2);
            return uindex;
        }

        public IList<object> PopResults(ScriptState luaState, int oldTopIndex)
        {
            int newTopIndex = LuaCore.GetTop(luaState);

            if (oldTopIndex == newTopIndex)
                return null;

            var returnValues = new List<object>();
            for (int i = oldTopIndex + 1; i <= newTopIndex; i++)
                returnValues.Add(DataHelper.GetObject(luaState, i));

            LuaCore.SetTop(luaState, oldTopIndex);
            return returnValues;
        }

        public void Dispose()
        {
            _cache.Clear();
            _mappings.Clear();
        }
    }
}
