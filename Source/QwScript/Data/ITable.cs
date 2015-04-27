using System;
using System.Collections.Generic;

namespace QwLua.Data
{
    public interface ITable : IEnumerable<KeyValuePair<object, object>>, IDisposable
    {
        void Refresh();
        object this[object fieldName] { get; set; }

        IList<T> ToList<T>();
    }
}
