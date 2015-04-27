using System.Collections.Generic;

namespace QwLua.Data
{
    public interface IFunctionResult
    {
        int ArgumentLength { get; }
        IList<object> Data { get; }
    }
}
