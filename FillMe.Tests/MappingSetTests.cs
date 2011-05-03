using System.Linq;
using System.Reflection;
using NUnit.Framework;
using Rhino.Mocks;

namespace FillMe.Tests
{
    [TestFixture]
    public class MappingSetTests
    {
        [Test]
        public void ShouldAddMappingForGivenPropertyUsingLambdasAndReflection()
        {
            var mappingSet = new MappingSet<Foo>();
            var mappingItemA = mappingSet.For(f => f.Name);
            var mappingItemB = mappingSet.For(typeof(Foo).GetProperty("Name", BindingFlags.Public | BindingFlags.Instance));
            Assert.That(mappingItemA, Is.EqualTo(mappingItemB));
            Assert.That(mappingSet.MappingItems.Count(), Is.EqualTo(1));
        }

        [Test]
        public void ShoulApplyDefaultGeneratorToMappingItemFromProvidedGeneratorFactory()
        {
            var generatorFactory = MockRepository.GenerateStub<IProvideDefaultGenerators>();
            var defaultGenerator = MockRepository.GenerateStub<IGenerateDummyData>();
            generatorFactory.Stub(g => g.GetFor(Arg<PropertyInfo>.Is.Anything)).Return(defaultGenerator);

            var mappingSet = new MappingSet<Foo>(generatorFactory);
            var mappingItemA = mappingSet.DefaultFor(f => f.Name);
            Assert.That(mappingItemA.Generator, Is.EqualTo(defaultGenerator));
        }

        [Test]
        public void ShouldSetDefaultGeneratorToAllStandardPropertiesFromDefaultGeneratorFactory()
        {
            var defaultGeneratorFactory = MockRepository.GenerateMock<IProvideDefaultGenerators>();
            var generator = MockRepository.GenerateStub<IGenerateDummyData>();

            defaultGeneratorFactory.Stub(f => f.GetFor(Arg<PropertyInfo>.Is.Anything)).Return(generator);

            var mappingSet = new MappingSet<Foo>(defaultGeneratorFactory);
            mappingSet.Defaults();

            Assert.That(mappingSet.Items.Count(), Is.EqualTo(6));
        }

        [Test]
        public void UsingDefaultsShouldNotOverridePresetGenerators()
        {
            var defaultGeneratorFactory = MockRepository.GenerateMock<IProvideDefaultGenerators>();
            var preSetGenerator = MockRepository.GenerateStub<IGenerateDummyData>();
            var generator = MockRepository.GenerateStub<IGenerateDummyData>();

            defaultGeneratorFactory.Stub(f => f.GetFor(Arg<PropertyInfo>.Is.Anything)).Return(generator);

            var mappingSet = new MappingSet<Foo>(defaultGeneratorFactory);
            mappingSet.For(f=> f.Age).Use(preSetGenerator);
            mappingSet.Defaults();

            Assert.That(mappingSet.Items.Any(i => i.Generator == preSetGenerator));
        }
    }
}