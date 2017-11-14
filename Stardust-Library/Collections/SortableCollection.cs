
using System.Collections.Generic;

namespace Stardust.Collections
{
    public class SortableCollection<T> where T: SortableElement
    {
        private readonly List<T> _elems;

        public SortableCollection()
        {
            _elems = new List<T>();
        }

        public List<T> Elems => _elems;

        public void Add(T elem)
        {
            if (_elems.Contains(elem)) return;
            _elems.Add(elem);
            elem.PriorityChange += SortElements;
            SortElements();
        }

        public void Remove(T elem)
        {
            bool removed = _elems.Remove(elem);
            if (removed)
            {
                elem.PriorityChange -= SortElements;
            }
        }

        public void Clear()
        {
            foreach (T action in _elems)
            {
                Remove(action);
            }
        }

        private void SortElements(SortableElement elem = null)
        {
            _elems.Sort(PriorityComparison);
        }

        // descending priority sort
        private static int PriorityComparison(T el1, T el2)
        {
            if (el1.Priority > el2.Priority)
            {
                return -1;
            }
            else if (el1.Priority < el2.Priority)
            {
                return 1;
            }
            return 0;
        }
    }
}