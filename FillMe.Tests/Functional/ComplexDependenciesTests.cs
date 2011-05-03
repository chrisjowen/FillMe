using System.Collections.Generic;
using FillIt;
using FillMe.DefaultGenerators;
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
            filler.Configure<Bar>().UseDefaults();
            filler.Configure<Foo>(config =>
            {
                config.For(foo => foo.Bars).Times(Constants.Random.Next(100));
                config.For(foo => foo.Age).Use(new RandomWholeNumberGenerator(10, 21)).Order(1);
                config.For(foo => foo.CalculatedAge).Do<User, int>(usr => usr.Friend.Age + 20).Order(2);
            }).UseDefaults();

            filler.Configure<Goo>().UseDefaults();
            filler.Configure<User>().UseDefaults();
            filler.Configure<AllowedPartner>(config =>
            {
                config.For(allowedPartner => allowedPartner.MinAge).Use(new MinAgeGenerator());
                config.For(allowedPartner => allowedPartner.MaxAge).Use(new MaxAgeGenerator());
            });

            1000.Times(() => users.Add(filler.Fill(new User())));
            users.ToString();
        }
    }


    public enum Sex
    {
        Male = 0,
        Female = 1
    }
}
