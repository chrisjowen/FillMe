using System;

namespace FillMe.Generators
{
    public class RandomEnumGenerator : IGenerateDummyData
    {
        private readonly Type type;

        public RandomEnumGenerator(Type type)
        {
            this.type = type;
        }

        public object Generate(GenerationContext context)
        {
            var enumValues = type.GetEnumValues();
            return enumValues.GetValue(Constants.Random.Next(0, enumValues.Length));
        }
    }
}