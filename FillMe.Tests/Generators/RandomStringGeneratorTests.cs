using FillMe.Generators;
using NUnit.Framework;

namespace FillMe.Tests.Generators
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
}
