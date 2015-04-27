using System;

namespace QwLua.Event
{
    public class ErrorEventArgs : EventArgs
    {
        private readonly Exception _exception;

        public Exception Exception
        {
            get { return _exception; }
        }

        public ErrorEventArgs(Exception ex)
        {
            _exception = ex;
        }
    }
}
