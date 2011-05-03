using System;
using System.Text;
using FillMe;


namespace FillIt
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
			return GenerateString(new Random().Next(min, max));
		}
		
		private string GenerateString(int size){
			var sb = new StringBuilder();
			size.Times(() => sb.Append(allowedChars.RandChar()));
			return sb.ToString();
		}
	}
}

