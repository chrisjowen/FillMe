using System.Reflection;
using FillIt;

namespace FillMe
{
	public class DefaultGeneratorFactory : IProvideDefaultGenerators
	{
		public IGenerateDummyData GetFor(PropertyInfo propertyInfo){
			return new RandomStringGenerator(0, 10);
		}
	}
}
