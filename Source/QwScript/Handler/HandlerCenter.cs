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