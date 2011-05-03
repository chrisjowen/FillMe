using System;
using System.Linq;
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


            filler.Configure<User>(config => config.For(user => user.Sex).Use(new SexGenerator())).UseDefaults();
            filler.Configure<AllowedPartner>(config =>
            {
                config.For(allowedPartner => allowedPartner.MinAge).Use(new MinAgeCalulator());
                config.For(allowedPartner => allowedPartner.MaxAge).Use(new MaxAgeCalulator());
            });


            1000.Times(() => users.Add(filler.Fill(new User())));

            users.ToString();
        }
    }

    public class MinAgeCalulator : IGenerateDummyData
    {
        public object Generate(object rootObject)
        {
            var user = ((User)rootObject);
            var timeDiference = DateTime.Now - user.Dob;
            var age = timeDiference.Days / 365;

            return user.Sex == Sex.Male
                ? GetMinAgeForMaleAged(age)
                : GetMinAgeForFemaleAged(age);
        }

        private static int GetMinAgeForFemaleAged(int age)
        {
            var minAgeForMaleAged = age / 1.5;
            return (int)Math.Round(minAgeForMaleAged > 16 ? minAgeForMaleAged : 16, 0);
        }

        private static int GetMinAgeForMaleAged(int age)
        {
            var minAgeForMaleAged = (age / 2) + 5;
            return minAgeForMaleAged > 16 ? minAgeForMaleAged : 16;
        }
    }


    public class MaxAgeCalulator : IGenerateDummyData
    {
        public object Generate(object rootObject)
        {
            var user = ((User)rootObject);
            var timeDiference = DateTime.Now - user.Dob;
            var age = timeDiference.Days / 365;

            return user.Sex == Sex.Male
                       ? GetMinAgeForMaleAged(age)
                       : GetMinAgeForFemaleAged(age);

        }

        private static int GetMinAgeForFemaleAged(int age)
        {
            var minAgeForMaleAged = age * 3;
            return minAgeForMaleAged > 16 ? minAgeForMaleAged : 16;
        }

        private static int GetMinAgeForMaleAged(int age)
        {
            var minAgeForMaleAged = age + 10;
            return minAgeForMaleAged > 16 ? minAgeForMaleAged : 16;
        }
    }

    public class SexGenerator : IGenerateDummyData
    {
        public object Generate(object rootObject)
        {
            return Enum.GetValues(typeof(Sex)).Cast<Sex>().ElementAt(new Random().Next(0, 2));
        }
    }

    public class User
    {
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Description { get; set; }
        public Foo Friend { get; set; }
        public Bar Enemy { get; set; }
        public Goo Maid { get; set; }
        public Sex Sex { get; set; }
        public DateTime Dob { get; set; }
        public AllowedPartner AllowedPartner { get; set; }
    }

    public enum Sex
    {
        Male = 0,
        Female = 1
    }

    public class AllowedPartner
    {
        public int MinAge { get; set; }
        public int MaxAge { get; set; }
    }
}
