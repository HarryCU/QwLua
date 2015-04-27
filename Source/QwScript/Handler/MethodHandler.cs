using QwLua.Data;
using QwLua.Implements;
using QwLua.Metatable;
using QwLua.Wrapper;

namespace QwLua.Handler
{
    internal class MethodHandler : BaseHandler
    {
        private MethodFinder _methodFinder;

        public MethodHandler(ScriptState luaState)
            : base(luaState)
        {
        }

        public MethodFinder Finder
        {
            get { return _methodFinder; }
        }

        public override void Initilaze(object data)
        {
            _methodFinder = data as MethodFinder;
        }

        public override int Reg2Env()
        {
            var luaState = State;
            var runtime = LuaRuntimePool.GetRuntime(luaState);
            var wrapper = new ScriptFunctionHandler(new MethodWrapper(Finder).Invoker);
            runtime.ObjectMgr.PushObjectEx(luaState, wrapper, FunctionMetatable.GlobalName);
            return 1;
        }
    }
}