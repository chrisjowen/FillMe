using System.Reflection;
using NUnit.Framework;
using Rhino.Mocks;

namespace FillMe.Tests
{
    [TestFixture]
    public class MappingItemTests
    {
        [Test]
        public void ShouldApplyGenerator()
        {
            var generator = MockRepository.GenerateStub<IGenerateDummyData>();
            var mappingItem = new MappingItem(GetFooProperty()).Use(generator);
            Assert.That(mappingItem.Generator, Is.EqualTo(generator));
        }

        private static PropertyInfo GetFooProperty()
        {
            return typeof (Foo).GetProperty("Name", BindingFlags.Public | BindingFlags.Instance);
        }
    }
}