using System;
using System.Text;
using FillIt;

namespace FillMe.DefaultGenerators
{
	public class RandomStringGenerator : IGenerateDummyData
	{
		private int min, max;
		private string allowedChars;
		
		private string alphaChars = "abcdefghijklmnopqrstuvwxyz";
		private string numericChars = "0123456789";
		
		public RandomStringGenerator (int min, int max)
		{
			this.min =min;
			this.max = max;
			allowedChars = alphaChars + numericChars;
		}
		
		public object Generate(object rootObject)
		{
            return GenerateString(Constants.Random.Next(min, max));
		}
		
		private string GenerateString(int size){
			var sb = new StringBuilder();
			size.Times(() => sb.Append(allowedChars.RandChar()));
			return sb.ToString();
		}
	}

    public class RandomEnumGenerator : IGenerateDummyData
    {
        private readonly Type type;

        public RandomEnumGenerator(Type type)
        {
            this.type = type;
        }

        public object Generate(object rootObject)
        {
            var enumValues = type.GetEnumValues();
            return enumValues.GetValue(Constants.Random.Next(0, enumValues.Length));
        }
    }
}

