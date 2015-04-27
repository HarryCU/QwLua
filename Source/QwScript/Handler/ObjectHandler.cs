using QwLua.Data;
using QwLua.Helpers;
using QwLua.Metatable;

namespace QwLua.Handler
{
    internal sealed class RObject
    {
        public string FullPath { get; private set; }
        public object Instance { get; private set; }

        public RObject(string fullPath, object instance)
        {
            FullPath = fullPath;
            Instance = instance;
        }
    }

    internal class ObjectHandler : BaseHandler
    {
        public RObject RObj { get; private set; }

        public ObjectHandler(ScriptState luaState)
            : base(luaState)
        {
        }

        public override void Initilaze(object data)
        {
            RObj = data as RObject;
        }

        public override int Reg2Env()
        {
            //var luaState = State;
            //var runtime = LuaRuntimePools.GetRuntime(luaState);
            ScriptHelper.SearchSetLuaObjectEx(State, RObj.FullPath, RObj.Instance, ObjectMetatable.GlobalName);
            //LuaCore.SetGlobal(luaState, RObj.Name);
            return 1;
        }
    }
}
