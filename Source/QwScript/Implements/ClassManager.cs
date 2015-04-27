using System;
using System.Reflection;
using QwLua.Attributes;
using QwLua.Data;
using QwLua.Handler;
using QwLua.Metatable;
using QwLua.Reflection;

namespace QwLua.Implements
{
    public sealed class ClassManager
    {
        private readonly MetatableInitilazer _metatables;

        private readonly ScriptState _luaState;
        private readonly HandlerCenter _cache;

        internal HandlerCenter Cache
        {
            get { return _cache; }
        }

        internal ClassManager() { }

        internal ClassManager(ScriptState luaState)
        {
            _luaState = luaState;
            _cache = new HandlerCenter();
            _metatables = new MetatableInitilazer(luaState);
            MetatableInitilaze();
        }

        private void MetatableInitilaze()
        {
            _metatables.Initilaze();
        }

        public void Register(Type type)
        {
            var classAttr = ReflectionHelper.GetAttributeOne<ScriptClassAttribute>(type);
            var registerName = type.Name;
            if (classAttr != null && !string.IsNullOrWhiteSpace(classAttr.Name))
            {
                registerName = classAttr.Name;
            }
            Register(type, registerName);
        }

        public void Register(Type type, string fullPath)
        {
            if (_cache.IsExist(type))
                return;

            if (ReflectionHelper.HasAttribute<ScriptIgnoreAttribute>(type))
                return;

            var handler = new TypeHandler(_luaState);
            handler.Initilaze(new object[] { type, fullPath });
            handler.Reg2Env();
            _cache.Add(type, handler);
        }

        public void ObjectRegister(object o)
        {
            var name = o.GetType().Name;
            ObjectRegister(name, o);
        }

        public void ObjectRegister(string name, object o)
        {
            if (_cache.IsExist(o))
                return;
            var handler = new ObjectHandler(_luaState);
            handler.Initilaze(new RObject(name, o));
            handler.Reg2Env();
            _cache.Add(o, handler);
        }

        public void AssemblyRegister(Assembly assembly)
        {
            foreach (var type in assembly.GetExportedTypes())
            {
                if (ReflectionHelper.HasAttribute<ScriptClassAttribute>(type))
                {
                    Register(type);
                }
            }
        }

        public void Dispose()
        {
            _cache.Dispose();
        }
    }
}
