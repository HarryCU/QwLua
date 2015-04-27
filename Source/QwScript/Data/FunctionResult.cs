using System.Collections.Generic;

namespace QwLua.Data
{
    public class FunctionResult : IFunctionResult
    {
        public FunctionResult(params object[] data)
        {
            Data = new List<object>(data);
            ArgumentLength = Data.Count;
        }

        public int ArgumentLength { get; private set; }
        public IList<object> Data { get; private set; }
    }
}