using System;
using NSubstitute;
using NUnit.Framework;
using pw.Generation;

namespace pw.Testing
{
    [TestFixture]
    public class Tests
    {
        private const string A = "a";
        private const string B = "b";
        private const string C = "c";
        private const string D = "d";

        [TestCase(A, ExpectedResult = 0)]
        [TestCase(B, ExpectedResult = 1)]
        [TestCase(C, ExpectedResult = 2)]
        [TestCase(D, ExpectedResult = 3)]
        public double Test(string password)
        {
            var model = Substitute.For<IProbabilisticPasswordModel>();
            model.GenerateSamplePassword().Returns(A, A, A, A, B, B, B, C, C, D);
            model.CalculateProbability(A).Returns(0.4);
            model.CalculateProbability(B).Returns(0.3);
            model.CalculateProbability(C).Returns(0.2);
            model.CalculateProbability(D).Returns(0.1);

            var sut = PasswordStrengthEvaluatorFactory.Create(10, model);

            return Math.Round(sut.Calculate(password), 15);
        }
    }
}
