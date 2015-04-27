using QwLua.Data;

namespace QwLua.Handler
{
    internal abstract class BaseHandler : IHandler
    {
        private readonly ScriptState _luaState;

        protected BaseHandler(ScriptState luaState)
        {
            _luaState = luaState;
        }

        protected ScriptState State
        {
            get { return _luaState; }
        }

        public abstract void Initilaze(object data);

        public abstract int Reg2Env();
    }
}
