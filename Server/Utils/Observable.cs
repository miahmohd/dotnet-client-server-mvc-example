using System.Collections.Generic;

namespace TrisGame
{
    public class Observable<T>
    {
        private List<IObserver<T>> _observers;

        protected Observable()
        {
            _observers = new List<IObserver<T>>();
        }

        public void AddObserver(IObserver<T> observer)
        {
            if (!_observers.Contains(observer))
                _observers.Add(observer);
        }

        public bool Unsubscribe(IObserver<T> observer)
        {
            return _observers.Remove(observer);
        }

        public void Notify(T obj)
        {
            foreach (var observer in _observers)
                observer.OnNotify(obj);
        }
    }
}