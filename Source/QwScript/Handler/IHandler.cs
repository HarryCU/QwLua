namespace QwLua.Handler
{
    internal interface IHandler : ILuaRegister
    {
        void Initilaze(object data);
    }
}
