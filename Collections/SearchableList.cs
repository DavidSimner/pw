using System.Collections.Generic;

namespace pw.Collections
{
    internal class SearchableList<T>
    {
        private readonly List<T> _list;

        internal SearchableList(List<T> list)
        {
            _list = list;
        }

        internal T this[int index] => _list[index];

        internal int BinarySearch(T item, IComparer<T> comparer)
        {
            return _list.BinarySearch(item, comparer);
        }
    }
}
