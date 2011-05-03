using System.Linq;
using NUnit.Framework;
using Rhino.Mocks;

namespace FillMe.Tests
{
    [TestFixture]
    public class FillerTests
    {
        [Test]
        public void ShouldAddMappingSetForGivenType()
        {
            var filler = new Filler();
            filler.Configure<Foo>();
            Assert.That(filler.mappingSets.Count(), Is.EqualTo(1));
            Assert.That(filler.mappingSets.First().Type, Is.EqualTo(typeof(Foo)));
        }

        [Test]
        public void ShouldApplyConfigurationActionToMappingSetItems()
        {
            var filler = new Filler();
            var generator = MockRepository.GenerateStub<IGenerateDummyData>();
            var generatoB = MockRepository.GenerateStub<IGenerateDummyData>();

            filler.Configure<Foo>(config => {
                config.For(f => f.Name).Use(generator);
                config.For(f => f.Age).Use(generatoB);
            });

            Assert.That(filler.mappingSets.Count(), Is.EqualTo(1));

            var mappingSet = filler.mappingSets.First() as MappingSet<Foo>;

            Assert.That(mappingSet.Type, Is.EqualTo(typeof(Foo)));
            Assert.That(mappingSet.MappingItems.Count(), Is.EqualTo(2));
            Assert.That(mappingSet.MappingItems.First().Value.Generator, Is.EqualTo(generator));
            Assert.That(mappingSet.MappingItems.Last().Value.Generator, Is.EqualTo(generatoB));
        }

        [Test]
        public void ShouldFillAllPropertiesThatHaveBeenSpecified()
        {
            const string dummyData = "Data";
            var rootObject = new Foo();
            var filler = new Filler();

            var generator = MockRepository.GenerateStub<IGenerateDummyData>();
            generator.Stub(g => g.Generate(rootObject)).Return(dummyData);

            filler.Configure<Foo>(config => {
                config.For(f => f.Name).Use(generator);
                config.For(f => f.Description).Use(generator);
            });


            filler.Fill(rootObject);

            Assert.That(rootObject.Name, Is.EqualTo(dummyData));
            Assert.That(rootObject.Description, Is.EqualTo(dummyData));
            Assert.That(rootObject.Info, Is.Null);
        }

    

    }
}
