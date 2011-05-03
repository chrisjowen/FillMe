using System;

namespace FillMe.Tests.Functional
{
    public class MaxAgeGenerator : IGenerateDummyData
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
}