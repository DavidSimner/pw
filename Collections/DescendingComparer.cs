using System;
using System.Collections.Generic;

namespace pw.Collections
{
    internal class DescendingComparer<T> : IComparer<T>
        where T : IComparable<T>
    {
        internal static readonly IComparer<T> Instance = new DescendingComparer<T>();

        int IComparer<T>.Compare(T x, T y)
        {
            return y.CompareTo(x);
        }
    }
}
