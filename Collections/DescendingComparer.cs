using System.Collections.Generic;

namespace pw.Collections
{
    internal class DescendingComparer : IComparer<double>
    {
        internal static readonly IComparer<double> Instance = new DescendingComparer();

        int IComparer<double>.Compare(double x, double y)
        {
            return y.CompareTo(x);
        }
    }
}
