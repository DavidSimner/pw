using System.Collections.Generic;
using pw.Collections;
using pw.Evaluation;

namespace pw.Generation
{
    internal static class PasswordStrengthEvaluatorFactory
    {
        internal static PasswordStrengthEvaluator Create(int n, IProbabilisticPasswordModel model)
        {
            var t = new List<double>(n + 1);
            t.Add(double.MaxValue);
            for (var i = 1; i <= n; ++i)
            {
                var samplePassword = model.GenerateSamplePassword();
                var sampleProbability = model.CalculateProbability(samplePassword);
                t.Add(sampleProbability);
            }

            var a = new SearchableList<double>(t, DescendingComparer.Instance);

            var c = new List<double>(n + 1);
            c.Add(0);
            for (var i = 1; i <= n; ++i)
            {
                c.Add(c[i - 1] + 1 / (n * a[i]));
            }

            return new PasswordStrengthEvaluator(model, a, c);
        }
    }
}
