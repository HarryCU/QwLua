namespace QwLua.Utils
{
    internal class BaseArgument
    {
        public BaseArgument(string script)
        {
            Script = script;
        }

        public string Script { get; private set; }
    }
}
