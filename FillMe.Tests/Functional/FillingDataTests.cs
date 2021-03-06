﻿using System;
using System.Linq;
using NUnit.Framework;
using Rhino.Mocks;

namespace FillMe.Tests.Functional
{
    [TestFixture]
    public class FillingDataTests
    {

        delegate object GeneratorDelegate(GenerationContext context);

        [Test]
        public void ShouldFillSubObject()
        {
            const string dummyData = "Data";
            var rootObject = new Foo();
            var filler = new Filler();

            var generator = MockRepository.GenerateStub<IGenerateDummyData>();
            generator.Stub(g => g.Generate(Arg<GenerationContext>.Is.Anything)).Return(dummyData);

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
            generator.Stub(g => g.Generate(Arg<GenerationContext>.Is.Anything)).Return(dummyData);

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
            generator.Stub(g => g.Generate(Arg<GenerationContext>.Is.Anything)).Return(10);

            var dependentPropertyGenerator = MockRepository.GenerateStub<IGenerateDummyData>();
            dependentPropertyGenerator.Stub(g => g.Generate(Arg<GenerationContext>.Is.Anything)).Do(new GeneratorDelegate(context => context.RootAs<Foo>().Age + 1));

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
            generator.Stub(g => g.Generate(Arg<GenerationContext>.Is.Anything)).Return(10);

            var dependentPropertyGenerator = MockRepository.GenerateStub<IGenerateDummyData>();
            dependentPropertyGenerator.Stub(g => g.Generate(Arg<GenerationContext>.Is.Anything)).Do(new GeneratorDelegate(context => context.RootAs<Foo>().Age + 1));

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
            generator.Stub(g => g.Generate(Arg<GenerationContext>.Is.Anything)).Return("Chris");

            var dependentGenerator = MockRepository.GenerateStub<IGenerateDummyData>();
            dependentGenerator.Stub(g => g.Generate(Arg<GenerationContext>.Is.Anything))
                .Do(new GeneratorDelegate(context => string.Format("Hello {0}", context.RootAs<Foo>().Bar.Name)));

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
            generator.Stub(g => g.Generate(Arg<GenerationContext>.Is.Anything)).Return("Chris");

            filler.Configure<Bar>(config => config.For(goo => goo.Name).Use(generator));
            filler.Configure<Foo>(config => config.For(f => f.Bars).Times(10));

            filler.Fill(rootObject);
            Assert.That(rootObject.Bars.Count, Is.EqualTo(10));
            Assert.That(rootObject.Bars.First().Name, Is.EqualTo("Chris"));
        }

        [Test]
        public void CollectionsGenerateDataBetweenXandYTimes()
        {
            var filler = new Filler();

            var generator = MockRepository.GenerateStub<IGenerateDummyData>();
            generator.Stub(g => g.Generate(Arg<GenerationContext>.Is.Anything)).Return("Chris");

            filler.Configure<Bar>(config => config.For(goo => goo.Name).Use(generator));
            filler.Configure<Foo>(config => config.For(f => f.Bars).Between(10, 20));

            for (var i = 0; i <= 100; i++)
            {
                var rootObject = new Foo();
                filler.Fill(rootObject);
                var bars = rootObject.Bars.Count;
                Assert.That(bars, Is.LessThanOrEqualTo(20));
                Assert.That(bars, Is.GreaterThanOrEqualTo(10));
            }
        }

    }
}