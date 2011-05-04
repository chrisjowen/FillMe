using FillMe.Generators;
using NUnit.Framework;

namespace FillMe.Tests.Generators
{
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
}