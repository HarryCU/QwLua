using QwLua.Data;
using QwLua.Helpers;
using QwLua.Implements;
using QwLua.Reflection;

namespace QwLua.Handler
{
    internal class ArrayHandler : BaseHandler, INewIndexer
    {
        private object _instance;
        private IMethod _method;
        private bool _getter;

        public ArrayHandler(ScriptState luaState)
            : base(luaState)
        {
        }

        private object Instance
        {
            get { return _instance; }
        }

        public override void Initilaze(object data)
        {
            var objs = data as object[];
            if (objs != null)
            {
                _instance = objs[0];
                _method = objs[1] as IMethod;
                _getter = (bool)objs[2];
            }
        }

        public override int Reg2Env()
        {
            var luaState = State;
            var runtime = LuaRuntimePool.GetRuntime(luaState);
            if (_getter)
            {
                var index = LuaCore.ToInt(luaState, -1);
                var val = _method.Invoke(Instance, index);
                runtime.ObjectMgr.PushObject(luaState, val);
                return 1;
            }
            DataHelper.PushObject(luaState, null);
            return 0;
        }

        public void SetValue(object value)
        {
            if (!_getter)
            {
                var index = LuaCore.ToInt(State, -2);
                _method.Invoke(Instance, index, value);
            }
        }
    }
}