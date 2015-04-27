using System.Threading;

namespace QwLua.Utils
{
    public abstract class BaseActuator : IActuator
    {
        private bool _paused;
        private readonly Thread _thread;

        public bool Paused
        {
            get
            {
                return _paused;
            }
        }

        protected BaseActuator()
        {
            _thread = new Thread(ThreadProc);
            _paused = false;
        }

        protected abstract void ThreadProc(object d);

        public void Start()
        {
            Start(null);
        }

        public void Start(object arg)
        {
            _thread.Start(arg);
        }

        public void Stop()
        {
            _thread.Abort();
        }

        public virtual void Pause()
        {
            _paused = true;
        }

        public virtual void Continue()
        {
            _paused = false;
        }
    }
}