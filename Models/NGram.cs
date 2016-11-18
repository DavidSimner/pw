using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using pw.Collections;

namespace pw.Models
{
    internal class NGram : Generation.IProbabilisticPasswordModel
    {
        private static readonly char[] Chars = new[] { PasswordSubstringIterator.Term }.Concat(Enumerable.Range(' ', '~' - ' ' + 1).Select(x => (char) x)).ToArray();
        private static readonly Random _random = new Random();

        private class Foo
        {
            internal static Foo Create(KeyValuePair<string, ConcurrentDictionary<char, int>> kvp)
            {
                return new Foo(kvp.Value);
            }

            private Foo(IDictionary<char, int> dict)
            {
                Dictionary = dict;

                Count = dict.Values.Sum();

                var list = new int[Chars.Length];
                for (var i = Chars.Length - 1; i >= 0; --i)
                {
                    var item = this[Chars[i]];
                    if (i != Chars.Length - 1)
                    {
                        item += list[i + 1];
                    }
                    list[i] = item;
                }
                SearchableList = new SearchableList<int>(new List<int>(list), DescendingComparer<int>.Instance);
            }

            private IDictionary<char, int> Dictionary { get; }
            internal int Count { get; }
            internal SearchableList<int> SearchableList { get; }

            internal int this[char c]
            {
                get
                {
                    int value;
                    return Dictionary.TryGetValue(c, out value) ? value : 0;
                }
            }
        }

        private readonly PasswordSubstringIterator _passwordSubstringIterator;
        private readonly IDictionary<string, Foo> _probabilities;

        internal NGram(int n, IEnumerable<KeyValuePair<string, int>> input)
        {
            _passwordSubstringIterator = new PasswordSubstringIterator(n);

            var counts = new ConcurrentDictionary<string, ConcurrentDictionary<char, int>>();
            foreach (var kvp in input)
            {
                _passwordSubstringIterator.Foreach(kvp.Key, (s, c) => counts.GetOrAdd(s, _ => new ConcurrentDictionary<char, int>()).AddOrUpdate(c, kvp.Value, (_, prev) => prev + kvp.Value));
            }

            _probabilities = counts.ToDictionary(x => x.Key, Foo.Create);
        }

        string Generation.IProbabilisticPasswordModel.GenerateSamplePassword()
        {
            var s = _passwordSubstringIterator.Begin;
            var g = new StringBuilder();
            while (true)
            {
                var r = _random.Next(_probabilities[s].Count);
                var i = _probabilities[s].SearchableList.BinarySearch(r);
                if (Chars[i] == PasswordSubstringIterator.Term)
                {
                    return g.ToString();
                }
                g.Append(Chars[i]);
                s = s.Substring(1) + Chars[i];
            }
        }

        double Evaluation.IProbabilisticPasswordModel.CalculateProbability(string password)
        {
            double ret = 1;
            _passwordSubstringIterator.Foreach(password, (s, c) => ret *= _probabilities[s][c] / (double) _probabilities[s].Count);
            return ret;
        }
    }
}
