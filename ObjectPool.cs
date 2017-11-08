namespace Stardust
{
using System;
using System.Collections.Generic;
using System.Threading;

    public class Pool<T> : IDisposable
    {
        private bool _isDisposed;
        private readonly Func<Pool<T>, T> _factory;
        private readonly Stack<T> _itemStore;
        
        public Pool(Func<Pool<T>, T> factory)
        {
            if (factory == null)
            {
                throw new ArgumentNullException(nameof(factory));    
            }
            _factory = factory;
            _itemStore = new Stack<T>();
        }

        public T Acquire()
        {
            if (_itemStore.Count > 0)
            {
                return _itemStore.Pop();
            }
            return _factory(this);
        }

        public void Release(T item)
        {
             _itemStore.Push(item);
        }

        /// <summary>
        /// Dispose the whole pool
        /// </summary>
        public void Dispose()
        {
            if (_isDisposed)
            {
                return;
            }
            _isDisposed = true;
            if (typeof(IDisposable).IsAssignableFrom(typeof(T)))
            {
                while (_itemStore.Count > 0)
                {
                    IDisposable disposable = (IDisposable)_itemStore.Pop();
                    disposable.Dispose();
                }
            }
        }
        
        public bool IsDisposed
        {
            get { return _isDisposed; }
        }
    }

}