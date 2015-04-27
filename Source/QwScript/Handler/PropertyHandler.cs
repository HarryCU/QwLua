using QwLua.Data;
using QwLua.Helpers;
using QwLua.Reflection;

namespace QwLua.Handler
{
    internal sealed class PropertyHandler : BaseHandler, INewIndexer
    {
        private IProperty _property;
        private object _instance;

        public PropertyHandler(ScriptState luaState)
            : base(luaState)
        {
        }

        public IProperty Property
        {
            get { return _property; }
        }

        public override void Initilaze(object data)
        {
            var objs = data as object[];
            if (objs != null)
            {
                _property = objs[0] as IProperty;
                _instance = objs[1];
            }
        }

        private object GetInstance()
        {
            object instance = null;
            if (!Property.IsStatic)
                instance = _instance;
            return instance;
        }

        public override int Reg2Env()
        {
            var instance = GetInstance();
            var value = Property.GetValue(instance);
            DataHelper.PushObject(State, value);
            return 1;
        }

        public void SetValue(object value)
        {
            var instance = GetInstance();
            Property.SetValue(instance, value);
        }
    }
}
