using System;

namespace FillMe.Tests.Functional
{
    public class MinAgeGenerator : IGenerateDummyData
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
}