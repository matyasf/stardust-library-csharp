
using System.Collections.Generic;

namespace Stardust.Collections
{
    public class SortableCollection
    {
        private readonly List<SortableElement> _elems;

        public SortableCollection()
        {
            _elems = new List<SortableElement>();
        }

        public IReadOnlyList<SortableElement> Elems => _elems;

        public void Add(SortableElement elem)
        {
            if (_elems.Contains(elem)) return;
            _elems.Add(elem);
            elem.PriorityChange += SortElements;
            SortElements();
        }

        public void Remove(SortableElement elem)
        {
            bool removed = _elems.Remove(elem);
            if (removed)
            {
                elem.PriorityChange -= SortElements;
            }
        }

        public void Clear()
        {
            foreach (SortableElement action in _elems)
            {
                Remove(action);
            }
        }

        private void SortElements(SortableElement elem = null)
        {
            _elems.Sort(PriorityComparison);
        }

        // descending priority sort
        private static int PriorityComparison(SortableElement el1, SortableElement el2)
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