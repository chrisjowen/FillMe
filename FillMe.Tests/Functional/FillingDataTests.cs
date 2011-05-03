using System;
using System.Linq;
using NUnit.Framework;
using Rhino.Mocks;

namespace FillMe.Tests.Functional
{
    [TestFixture]
    public class FillingDataTests
    {

        delegate object GeneratorDelegate(object rootObject);

        [Test]
        public void ShouldFillSubObject()
        {
            const string dummyData = "Data";
            var rootObject = new Foo();
            var filler = new Filler();

            var generator = MockRepository.GenerateStub<IGenerateDummyData>();
            generator.Stub(g => g.Generate(rootObject)).Return(dummyData);

            filler.Configure<Foo>();
            filler.Configure<Bar>(config => config.For(f => f.Name).Use(generator));

            filler.Fill(rootObject);

            Assert.That(rootObject.Bar, Is.Not.Null);
            Assert.That(rootObject.Bar.Name, Is.EqualTo(dummyData));
        }

        [Test]
        public void ShouldNotFillSubObjectIfRootObjectNotDefined()
        {
            const string dummyData = "Data";
            var rootObject = new Foo();
            var filler = new Filler();

            var generator = MockRepository.GenerateStub<IGenerateDummyData>();
            generator.Stub(g => g.Generate(rootObject)).Return(dummyData);

            filler.Configure<Bar>(config => config.For(f => f.Name).Use(generator));

            filler.Fill(rootObject);
            Assert.That(rootObject.Bar, Is.Null);
        }

        [Test]
        public void ShouldUsePrefilledDependentPropertyToCalculateValue()
        {
            var rootObject = new Foo();
            var filler = new Filler();

            var generator = MockRepository.GenerateStub<IGenerateDummyData>();
            generator.Stub(g => g.Generate(rootObject)).Return(10);

            var dependentPropertyGenerator = MockRepository.GenerateStub<IGenerateDummyData>();
            dependentPropertyGenerator.Stub(g => g.Generate(rootObject)).Do(new GeneratorDelegate(root => ((Foo)root).Age + 1));

            filler.Configure<Foo>(config =>
            {
                config.For(f => f.Age).Use(generator);
                config.For(f => f.CalculatedAge).Use(dependentPropertyGenerator);
            });

            filler.Fill(rootObject);
            Assert.That(rootObject.Age, Is.EqualTo(10));
            Assert.That(rootObject.CalculatedAge, Is.EqualTo(11));
        }

        [Test]
        public void DependentFieldsWillNotBeUsefulUnlessOrderIsSpecified()
        {
            var rootObject = new Foo();
            var filler = new Filler();

            var generator = MockRepository.GenerateStub<IGenerateDummyData>();
            generator.Stub(g => g.Generate(rootObject)).Return(10);

            var dependentPropertyGenerator = MockRepository.GenerateStub<IGenerateDummyData>();
            dependentPropertyGenerator.Stub(g => g.Generate(rootObject)).Do(new GeneratorDelegate(root => ((Foo)root).Age + 1));

            filler.Configure<Foo>(config =>
            {
                config.For(f => f.CalculatedAge).Use(dependentPropertyGenerator);
                config.For(f => f.Age).Use(generator);
            });

            filler.Fill(rootObject);
            Assert.That(rootObject.Age, Is.EqualTo(10));
            Assert.That(rootObject.CalculatedAge, Is.EqualTo(1));

            filler.Configure<Foo>(config =>
            {
                config.For(f => f.Age).Order(2);
                config.For(f => f.CalculatedAge).Order(1);
            });

            filler.Fill(rootObject);
            Assert.That(rootObject.Age, Is.EqualTo(10));
            Assert.That(rootObject.CalculatedAge, Is.EqualTo(11));
        }

        [Test]
        public void DependencyOrderingWorksWithSubclassesAsWellAsPrimitives()
        {
            var rootObject = new Foo();
            var filler = new Filler();

            var generator = MockRepository.GenerateStub<IGenerateDummyData>();
            generator.Stub(g => g.Generate(rootObject)).Return("Chris");

            var dependentGenerator = MockRepository.GenerateStub<IGenerateDummyData>();
            dependentGenerator.Stub(g => g.Generate(rootObject)).Do(new GeneratorDelegate(root => string.Format("Hello {0}", ((Foo)root).Bar.Name)));

            filler.Configure<Goo>(config => config.For(goo => goo.Name).Use(dependentGenerator));
            filler.Configure<Bar>(config => config.For(goo => goo.Name).Use(generator));

            filler.Configure<Foo>(config =>
            {
                config.For(f => f.Goo).Order(2);
                config.For(f => f.Bar).Order(1);
            });

            filler.Fill(rootObject);

            Assert.That(rootObject.Goo.Name, Is.EqualTo("Hello Chris"));
        }

        [Test]
        public void CollectionsGenerateDataXTimes()
        {
            var rootObject = new Foo();
            var filler = new Filler();

            var generator = MockRepository.GenerateStub<IGenerateDummyData>();
            generator.Stub(g => g.Generate(rootObject)).Return("Chris");

            filler.Configure<Bar>(config => config.For(goo => goo.Name).Use(generator));
            filler.Configure<Foo>(config => config.For(f => f.Bars).Times(10));

            filler.Fill(rootObject);
            Assert.That(rootObject.Bars.Count, Is.EqualTo(10));
            Assert.That(rootObject.Bars.First().Name, Is.EqualTo("Chris"));
        }

    }
}