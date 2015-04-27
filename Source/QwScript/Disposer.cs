using System;

namespace QwLua
{
    public abstract class Disposer : IDisposable
    {
        private volatile bool _disposed;

        ~Disposer()
        {
            Dispose(false);
            GC.SuppressFinalize(this);
        }

        protected bool Disposed
        {
            get
            {
                return _disposed;
            }
        }

        protected virtual bool ReleasePrevious()
        {
            return true;
        }

        protected abstract void Release();

        private void Dispose(bool disposing)
        {
            if (disposing && ReleasePrevious() && !_disposed)
            {
                Release();
                _disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
    }
}
