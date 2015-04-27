using System;
using System.ComponentModel;

namespace QwLua.Core
{
    public sealed class EventManager : Disposer
    {
        private EventManager() { }

        private readonly EventHandlerList _eventHandlers = new EventHandlerList();

        public void Add(object key, Delegate @delegate)
        {
            _eventHandlers.AddHandler(key, @delegate);
        }

        public void Remove(object key, Delegate @delegate)
        {
            _eventHandlers.RemoveHandler(key, @delegate);
        }

        public T GetHandler<T>(object key)
        {
            return (T)(object)_eventHandlers[key];
        }

        public EventHandler<T> GetEventHandler<T>(object key)
        {
            return _eventHandlers[key] as EventHandler<T>;
        }

        public static EventManager Create()
        {
            return new EventManager();
        }

        protected override void Release()
        {
            _eventHandlers.Dispose();
        }
    }
}