using System;
using System.Reflection;
using FillMe.DefaultGenerators;
using NUnit.Framework;

namespace FillMe.Tests
{
    [TestFixture]
    public class DefaultGeneratorFactoryTests
    {
        private IProvideDefaultGenerators factory;

        [SetUp]
        public void BeforeEachTest()
        {
            factory = new DefaultGeneratorFactory();
        }

        [Test]
        public void ShouldReturnRandomTextGeneratorForStringProperties()
        {
            Assert.That(GetGeneratorFor("StringProperty"), Is.TypeOf(typeof(RandomStringGenerator)));
        }

        [Test]
        public void ShouldReturnRandomWholeNumberGeneratorForIntProperties()
        {
            Assert.That(GetGeneratorFor("IntProperty"), Is.TypeOf(typeof(RandomWholeNumberGenerator)));
        }

        [Test]
        public void ShouldReturnRandomDecmialNumberNumberGeneratorForDecimalProperties()
        {
            Assert.That(GetGeneratorFor("DecimalProperty"), Is.TypeOf(typeof(RandomDecimalNumberGenerator)));
        }        
        
        [Test]
        public void ShouldReturnRandomDateTimeGeneratorForDateTimeProperties()
        {
            Assert.That(GetGeneratorFor("DateTimeProperty"), Is.TypeOf(typeof(RandomDateTimeGenerator)));
        }

        public class FooBar
        {
            public string StringProperty { get; set; }
            public int IntProperty { get; set; }
            public decimal DecimalProperty { get; set; }
            public DateTime DateTimeProperty { get; set; }
        }

        public IGenerateDummyData GetGeneratorFor(string propertyName)
        {
            var propertyInfo = typeof (FooBar).GetProperty(propertyName, BindingFlags.Instance | BindingFlags.Public);
            return factory.GetFor(propertyInfo); ;
        }

    }

  
}
