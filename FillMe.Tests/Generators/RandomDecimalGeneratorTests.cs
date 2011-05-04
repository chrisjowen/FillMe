using System;
using System.Linq;
using FillMe.Generators;
using NUnit.Framework;

namespace FillMe.Tests.Generators
{
    [TestFixture]
    public class RandomDecimalGeneratorTests
    {
        [Test]
        public void ShouldGenerateUniquDecimalNumberBetween0And100CharsLong()
        {
            for (var i = 0; i <= 1000; i++)
            {
                var generated = new RandomDecimalNumberGenerator(0, 100).Generate(null);
                Assert.That(generated, Is.LessThanOrEqualTo(100));
                Assert.That(generated, Is.GreaterThanOrEqualTo(0));
            }
        }

        [Test]
        public void ShouldGenerateUniquDecimalsWithGivenPrecision()
        {
            for (var i = 0; i <= 1000; i++)
            {
                var generated = new RandomDecimalNumberGenerator(0, 100).WithPrecision(5).Generate(null).ToString();
                var precision = generated.Split(new[] {"."}, StringSplitOptions.None).Last().Length;
                Assert.That(precision, Is.LessThanOrEqualTo(5));
            }
        }

    }
}