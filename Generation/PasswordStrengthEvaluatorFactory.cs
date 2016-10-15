using System.Collections.Generic;
using pw.Collections;
using pw.Evaluation;

namespace pw.Generation
{
    internal static class PasswordStrengthEvaluatorFactory
    {
        internal static PasswordStrengthEvaluator Create(int n, IProbabilisticPasswordModel model)
        {
            var a = new List<double>(n);
            for (var i = 0; i < n; ++i)
            {
                var samplePassword = model.GenerateSamplePassword();
                var sampleProbability = model.CalculateProbability(samplePassword);
                a.Add(sampleProbability);
            }
            a.Sort(DescendingComparer.Instance);

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
