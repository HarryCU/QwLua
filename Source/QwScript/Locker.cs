using System.Threading;

namespace QwLua
{
    public class Locker : Disposer
    {
        private readonly object _lockObject = new object();

        private bool _locked;

        public Locker()
            : this(false)
        {
        }

        public Locker(bool @lock)
        {
            if (@lock)
                Lock();
        }

        public bool Lock()
        {
            Monitor.Enter(_lockObject);
            _locked = true;
            return _locked;
        }

        public void Unlock()
        {
            if (_locked)
            {
                _locked = false;
                Monitor.Exit(_lockObject);
            }
        }

        protected override void Release()
        {
            Unlock();
        }
    }
}
