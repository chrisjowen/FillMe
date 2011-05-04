using System;

namespace FillMe.Generators
{
    public class RandomDateTimeGenerator : IGenerateDummyData
    {
        public object Generate(GenerationContext context)
        {
            return new DateTime(Constants.Random.Next(1900, 2011), Constants.Random.Next(1, 12), Constants.Random.Next(1, 28));
        }
    }
}