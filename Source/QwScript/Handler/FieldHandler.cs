using QwLua.Data;
using QwLua.Helpers;
using QwLua.Reflection;

namespace QwLua.Handler
{
    internal sealed class FieldHandler : BaseHandler, INewIndexer
    {
        private IField _field;
        private object _instance;

        public FieldHandler(ScriptState luaState)
            : base(luaState)
        {
        }

        private IField Field
        {
            get { return _field; }
        }

        public override void Initilaze(object data)
        {
            var objs = data as object[];
            if (objs != null)
            {
                _field = objs[0] as IField;
                _instance = objs[1];
            }
        }

        private object GetInstance()
        {
            object instance = null;
            if (!Field.IsStatic)
                instance = _instance;
            return instance;
        }

        public override int Reg2Env()
        {
            var instance = GetInstance();
            var value = Field.GetValue(instance);
            DataHelper.PushObject(State, value);
            return 1;
        }

        public void SetValue(object value)
        {
            var instance = GetInstance();
            Field.SetValue(instance, value);
        }
    }
}
