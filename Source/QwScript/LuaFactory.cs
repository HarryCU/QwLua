using QwLua.Implements;

namespace QwLua
{
    public static class LuaFactory
    {
        public static ILuaRuntime CreateRuntime()
        {
            return new LuaRuntime();
        }

        public static ILuaRuntime CreateRuntime(object mark)
        {
            return new LuaRuntime(mark);
        }
    }
}