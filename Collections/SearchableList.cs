using System.Collections.Generic;

namespace pw.Collections
{
    internal class SearchableList<T>
    {
        private readonly List<T> _list;
        private readonly IComparer<T> _comparer;

        internal SearchableList(List<T> list, IComparer<T> comparer)
        {
            list.Sort(comparer);

            _list = list;
            _comparer = comparer;
        }

        internal T this[int index] => _list[index];

        internal int BinarySearch(T item)
        {
            var index = _list.BinarySearch(item, _comparer);
            return index - 1;
        }
    }
}
