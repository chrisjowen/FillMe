using System;
using System.Text;
using System.Reflection;

namespace FillIt
{
	public class DefaultGeneratorFactory : IProvideDefaultGenerators
	{
		public IGenerateDummyData GetFor(PropertyInfo propertyInfo){
			return new RandomStringGenerator(0, 10);
		}
	}
}
