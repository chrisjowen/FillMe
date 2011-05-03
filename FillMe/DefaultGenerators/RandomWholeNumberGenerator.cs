using System;

namespace FillMe.DefaultGenerators
{
    public class RandomWholeNumberGenerator : IGenerateDummyData
    {
        private readonly int min;
        private readonly int max;

        public RandomWholeNumberGenerator(int min, int max)
        {
            this.min = min;
            this.max = max;
        }

        public object Generate(object rootObject)
        {
            return Constants.Random.Next(min, max);
        }
    }

    public class RandomDecimalNumberGenerator : IGenerateDummyData
    {
        private readonly int min;
        private readonly int max;
        private int precision;

        public RandomDecimalNumberGenerator(int min, int max)
        {
            this.min = min;
            this.max = max;
            precision = 2;
        }

        public object Generate(object rootObject)
        {
            return Math.Round(Constants.Random.Next(min, max-1) + Constants.Random.NextDouble(), precision);
        }

        public RandomDecimalNumberGenerator WithPrecision(int precision)
        {
            this.precision = precision;
            return this;
        }
    }

    public class RandomDateTimeGenerator : IGenerateDummyData
    {
        public object Generate(object rootObject)
        {
            return new DateTime(Constants.Random.Next(1900, 2011), Constants.Random.Next(1, 12), Constants.Random.Next(1, 28));
        }
    }
}