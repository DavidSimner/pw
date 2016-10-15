using System.Collections.Generic;
using pw.Collections;

namespace pw.Evaluation
{
    internal class PasswordStrengthEvaluator
    {
        private readonly IProbabilisticPasswordModel _model;
        private readonly SearchableList<double> _a;
        private readonly List<double> _c;

        internal PasswordStrengthEvaluator(IProbabilisticPasswordModel model, SearchableList<double> a, List<double> c)
        {
            _model = model;
            _a = a;
            _c = c;
        }

        internal double Calculate(string password)
        {
            var probability = _model.CalculateProbability(password);
            var index = _a.BinarySearch(probability, DescendingComparer.Instance);
            return _c[index - 1];
        }
    }
}
