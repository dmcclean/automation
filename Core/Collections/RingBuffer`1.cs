using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AutomationLibrary.Collections
{
    public class RingBuffer<TItem>
        : IEnumerable<TItem>
    {
        private readonly TItem[] _items;
        private readonly int _capacity;
        private int _count;
        private int _position; // this is the physical position of the logically zeroth element

        public RingBuffer(int capacity)
        {
            if (capacity <= 0) throw new ArgumentOutOfRangeException();
            _items = new TItem[capacity];
            _capacity = capacity;
            _count = 0;
            _position = 0;
        }

        public int Count
        {
            get
            {
                return _count;
            }
        }

        public int Capacity
        {
            get
            {
                return _capacity;
            }
        }

        public void Add(TItem item)
        {
            // index of next position to fill
            var insertionPosition = (_position + _count) % _capacity;
            var atCapacity = _count == _capacity;
            _items[insertionPosition] = item;
            if (!atCapacity)
            {
                // position remains the same, count grows
                _count += 1;
            }
            else
            {
                // count remains the same, position grows
                _position = (_position + 1) % _capacity;
            }
        }

        public void AddRange(IEnumerable<TItem> items)
        {
            foreach (var item in items)
            {
                Add(item);
            }
        }

        public void AddRange(ArraySegment<TItem> items)
        {
            // TODO: make an efficient Array.Copy implementation
            for (int i = items.Offset; i < items.Offset + items.Count; i++)
            {
                Add(items.Array[i]);
            }
        }

        public void AddRange(TItem[] items)
        {
            var segment = new ArraySegment<TItem>(items);
            
            AddRange(segment);
        }

        public void Clear()
        {
            _count = 0;
            _position = 0;

            // let the garbage collector cleanup so there aren't any GC leak surprises
            if (!typeof(TItem).IsValueType)
            {
                for (int i = 0; i < _capacity; i++)
                {
                    _items[i] = default(TItem);
                }
            }
        }

        public void CopyTo(TItem[] destination, int position)
        {
            int lengthOfFirstSegment = Math.Min(_count, _capacity - _position);
            int lengthOfSecondSegment = Math.Max(0, _count - lengthOfFirstSegment);

            int lengthOfDestination = destination.Length - position;

            int lengthOfFirstCopy = Math.Min(lengthOfFirstSegment, lengthOfDestination);
            Array.Copy(_items, _position, destination, position, lengthOfFirstCopy);

            lengthOfDestination -= lengthOfFirstSegment;
            int lengthOfSecondCopy = Math.Min(lengthOfSecondSegment, lengthOfDestination);
            Array.Copy(_items, 0, destination, position + lengthOfFirstSegment, lengthOfSecondCopy);            
        }

        public IEnumerator<TItem> GetEnumerator()
        {
            int lengthOfFirstSegment = Math.Min(_count, _capacity - _position);
            int lengthOfSecondSegment = Math.Max(0, _count - lengthOfFirstSegment);
            for (int i = _position; i < _position + lengthOfFirstSegment; i++) yield return _items[i];
            for (int i = 0; i < lengthOfSecondSegment; i++) yield return _items[i];
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
