namespace FillMe.Generators
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

        public object Generate(GenerationContext context)
        {
            return Constants.Random.Next(min, max);
        }
    }
}