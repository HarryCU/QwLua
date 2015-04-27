using System;

namespace QwLua.Data
{
    internal abstract class ScriptDataBase : IDisposable
    {
        ~ScriptDataBase()
        {
            Dispose(false);
            GC.SuppressFinalize(this);
        }

        protected abstract void Dispose(bool disposing);

        public void Dispose()
        {
            Dispose(true);
        }
    }
}
