using System;

namespace FillMe.Generators
{
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

        public object Generate(GenerationContext context)
        {
            return Math.Round(Constants.Random.Next(min, max-1) + Constants.Random.NextDouble(), precision);
        }

        public RandomDecimalNumberGenerator WithPrecision(int precision)
        {
            this.precision = precision;
            return this;
        }
    }
}