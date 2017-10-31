namespace Stardust.Math
{
using System;
using System.Collections.Generic;
using System.Threading;

    public class Pool<T> : IDisposable
    {
        private bool _isDisposed;
        private readonly Func<Pool<T>, T> _factory;
        private readonly Stack<T> _itemStore;
        private readonly int _size;
        private int _count;
        private readonly Semaphore _sync;
        
        public Pool(int size, Func<Pool<T>, T> factory)
        {
            if (size <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(size), size,
                    "Argument 'size' must be greater than zero.");
            }
            if (factory == null)
            {
                throw new ArgumentNullException(nameof(factory));    
            }
            _size = size;
            _factory = factory;
            _sync = new Semaphore(size, size);
            _itemStore = new Stack<T>(size);
        }

        public T Acquire()
        {
            _sync.WaitOne();
            bool shouldExpand = false;
            if (_count < _size)
            {
                int newCount = Interlocked.Increment(ref _count);
                if (newCount <= _size)
                {
                    shouldExpand = true;
                }
                else
                {
                    // Another thread took the last spot - use the store instead
                    Interlocked.Decrement(ref _count);
                }
            }
            if (shouldExpand)
            {
                return _factory(this);
            }
            lock (_itemStore)
            {
                return _itemStore.Pop();
            }
        }

        public void Release(T item)
        {
            lock (_itemStore)
            {
                _itemStore.Push(item);
            }
            _sync.Release();
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
                lock (_itemStore)
                {
                    while (_itemStore.Count > 0)
                    {
                        IDisposable disposable = (IDisposable)_itemStore.Pop();
                        disposable.Dispose();
                    }
                }
            }
            _sync.Close();
        }
        
        public bool IsDisposed
        {
            get { return _isDisposed; }
        }
    }

}