using System;
using System.Collections;
using System.Collections.Generic;
using QwLua.Helpers;
using QwLua.Implements;

namespace QwLua.Data
{
    [Flags]
    public enum ReadTableWay
    {
        ByName = 1,
        ByIndex = 2
    }

    internal sealed class Table : ScriptDataBase, ITable
    {
        private readonly ReadTableWay _readTableWay;
        private readonly ScriptState _luaState;
        private readonly int _index;
        private readonly IDictionary<object, object> _cache = new Dictionary<object, object>(10);

        public Table(ScriptState luaState, string tableName)
        {
            _luaState = luaState;
            _index = LuaCore.GetGlobalRef(_luaState, tableName, ScriptTypes.Table);
            _readTableWay = ReadTableWay.ByName;
            Refresh();
        }

        public Table(ScriptState luaState, int index)
        {
            _luaState = luaState;
            _index = LuaCore.GetRef(_luaState, index);
            _readTableWay = ReadTableWay.ByName;
            Refresh();
        }

        public ReadTableWay Way
        {
            get { return _readTableWay; }
        }

        public void Refresh()
        {
            _cache.Clear();

            Action<ScriptState> loopAction = l =>
            {
                var key = DataHelper.GetObject(l, -2);
                var value = DataHelper.GetObject(l, -1);
                _cache.Add(key, value);
            };

            LuaCore.ResetTop(_luaState, l => LuaCore.ForeachTable(l, _index, loopAction));
        }

        public object this[object fieldName]
        {
            get
            {
                if (_cache.ContainsKey(fieldName))
                    return _cache[fieldName];
                return null;
            }
            set
            {
                var val = value;

                LuaCore.ResetTop(_luaState,
                    ls => LuaCore.SetTableValue(ls, _index, l =>
                    {
                        DataHelper.PushObject(l, fieldName);
                        DataHelper.PushObject(l, val);
                    }));
            }
        }

        public IList<T> ToList<T>()
        {
            IList<T> list = new List<T>(_cache.Count);
            foreach (var value in _cache.Values)
            {
                list.Add((T)value);
            }
            return list;
        }

        public IEnumerator<KeyValuePair<object, object>> GetEnumerator()
        {
            return _cache.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _cache.Clear();
            }
        }

        internal void Push(ScriptState luaState)
        {
            LuaCore.GlobalRawGet(luaState, _index);
        }
    }
}