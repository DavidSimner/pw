using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using NUnit.Framework;
using pw.Generation;
using pw.Models;

namespace pw.Testing
{
    [TestFixture]
    public class RockYou
    {
        private static readonly Regex Regex = new Regex("^[ !\"#$%&'()*+,-./0-9:;<=>?@A-Z[\\\\\\]^_`a-z{|}~]*$", RegexOptions.Compiled);

        [Test]
        public void Goo()
        {
            var model = new NGram(4, GetFilePasswordsAndCounts("rockyou-withcount.txt", Regex));
            var evaluator = PasswordStrengthEvaluatorFactory.Create(10, model);

            var rank0 = evaluator.Calculate("123456");
            Assert.That(rank0, Is.EqualTo(0));
            var rank1 = evaluator.Calculate("12345");
            Assert.That(rank1, Is.EqualTo(0));
        }

        private static IEnumerable<KeyValuePair<string, int>> GetFilePasswordsAndCounts(string path, Regex regex)
        {
            foreach (var line in File.ReadLines(path).Take(short.MaxValue))
            {
                if (!regex.IsMatch(line))
                {
                    continue;
                }

                var trimmedLine = line.TrimStart();
                var index = trimmedLine.IndexOf(" ", StringComparison.OrdinalIgnoreCase);
                var count = trimmedLine.Substring(0, index);
                var password = trimmedLine.Substring(index + 1);

                yield return new KeyValuePair<string, int>(password, int.Parse(count));
            }
        }
    }
}
