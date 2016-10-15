using System.Collections.Generic;

namespace pw.Collections
{
    internal class DescendingComparer : IComparer<double>
    {
        internal static readonly IComparer<double> Instance = new DescendingComparer();

        public int Compare(double x, double y)
        {
            return y.CompareTo(x);
        }
    }
}
