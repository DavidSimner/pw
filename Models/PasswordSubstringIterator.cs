using System;

namespace pw.Models
{
    internal class PasswordSubstringIterator
    {
        internal const char Term = '⊥';

        private readonly int _n;
        private readonly int _nMinus1;
        private readonly string _begin;
        private readonly string _end;

        internal PasswordSubstringIterator(int n)
        {
            _n = n;
            _nMinus1 = n - 1;
            _begin = new string(Term, _nMinus1);
            _end = Term.ToString();
        }

        internal string Begin => _begin;

        internal void Foreach(string password, Action<string, char> action)
        {
            password = _begin + password + _end;

            for (var i = 0; i <= password.Length - _n; ++i)
            {
                action(password.Substring(i, _nMinus1), password[i + _nMinus1]);
            }
        }
    }
}
