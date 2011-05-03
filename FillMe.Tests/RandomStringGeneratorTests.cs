using System;
using System.Linq;
using FillMe.DefaultGenerators;
using NUnit.Framework;

namespace FillMe.Tests
{
    [TestFixture]
    public class RandomStringGeneratorTests
    {
        [Test]
        public void ShouldGenerateUniqueTextBetween5And10CharsLong()
        {
            var lastText = "";
            for (var i = 0; i <= 1000; i++)
            {
                var generated = new RandomStringGenerator(5, 10).Generate(null).ToString();
                Assert.That(generated.Length, Is.LessThanOrEqualTo(10));
                Assert.That(generated.Length, Is.GreaterThanOrEqualTo(5));
                Assert.AreNotEqual(lastText, generated);
                lastText = generated;
            }
        }
    }    
    
    [TestFixture]
    public class RandomWholeGeneratorTests
    {
        [Test]
        public void ShouldGenerateUniqueWholeNumberBetween0And100CharsLong()
        {
            for (var i = 0; i <= 1000; i++)
            {
                var generated = new RandomWholeNumberGenerator(0, 100).Generate(null);
                Assert.That(generated, Is.LessThanOrEqualTo(100));
                Assert.That(generated, Is.GreaterThanOrEqualTo(0));
            }
        }
    }

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
