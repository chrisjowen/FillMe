using System.Collections.Generic;
using FillIt;
using FillMe.Generators;
using NUnit.Framework;

namespace FillMe.Tests.Functional
{
    [TestFixture]
    public class ComplexDependenciesTests
    {
        [Test]
        public void LetsGetSomeMatchesGoingOn()
        {
            var users = new List<User>();
            
            var filler = new Filler();
            filler.Configure<Bar>().Defaults();
            filler.Configure<Foo>(config =>
            {
                config.For(foo => foo.Bars).Times(Constants.Random.Next(100));
                config.For(foo => foo.Age).Use(new RandomWholeNumberGenerator(10, 21)).Order(1);
                config.For(foo => foo.CalculatedAge).Do(context => context.CurrentAs<Foo>().Age + 20).Order(2);
            }).Defaults();

            filler.Configure<Goo>().Defaults();
            filler.Configure<User>().Defaults();
            filler.Configure<AllowedPartner>(config =>
            {
                config.For(allowedPartner => allowedPartner.MinAge).Use(new MinAgeGenerator());
                config.For(allowedPartner => allowedPartner.MaxAge).Use(new MaxAgeGenerator());
            });

            1000.Times(() => users.Add(filler.Fill(new User())));
            users.ToString();
        }
    }
}
