using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Homework3
{
    class BoundBuffer<T>
    {
        private const int MaxSize = 4000000;
        private Queue<T> _queue;
        private Semaphore _full;
        private Semaphore _empty;
        private Semaphore _mutex;
        public int Count { get; }
        public BoundBuffer()
        {
            _queue = new Queue<T>();
            _mutex = new Semaphore(1, 1);
            _full = new Semaphore(0, MaxSize);
            _empty = new Semaphore(MaxSize, MaxSize);
        }

        public T Dequeue()
        {
            _full.WaitOne();
            _mutex.WaitOne();
            var item = _queue.Dequeue();
            _mutex.Release(1);
            _empty.Release(1);
            return item;
        }

        public void Enqueue(T item)
        {
            _empty.WaitOne();
            _mutex.WaitOne();
            _queue.Enqueue(item);
            _mutex.Release(1);
            _full.Release(1);
        }

    }
}
