using System.Collections.Generic;
using pw.Collections;
using pw.Evaluation;

namespace pw.Generation
{
    internal static class PasswordStrengthEvaluatorFactory
    {
        internal static PasswordStrengthEvaluator Create(int n, IProbabilisticPasswordModel model)
        {
            var t = new List<double>(n);
            for (var i = 0; i < n; ++i)
            {
                var samplePassword = model.GenerateSamplePassword();
                var sampleProbability = model.CalculateProbability(samplePassword);
                t.Add(sampleProbability);
            }
            t.Sort(DescendingComparer.Instance);

            var a = new SearchableList<double>(t);

            var c = new List<double>(n);
            for (var i = 0; i < n; ++i)
            {
                c.Add(1 / (n * a[i]));
                if (i >= 1)
                {
                    c[i] += c[i - 1];
                }
            }

            return new PasswordStrengthEvaluator(model, a, c);
        }
    }
}
