using System;
using System.Collections.Generic;

namespace QwLua.Data
{
    public interface IScriptFunction : IDisposable
    {
        IList<object> Call(params object[] args);
    }
}
