using System;
using System.Reflection;
using FillMe.Generators;

namespace FillMe.Generators
{
	public class DefaultGeneratorFactory : IProvideDefaultGenerators
	{
		public IGenerateDummyData GetFor(PropertyInfo propertyInfo)
		{
		    var propertyType = propertyInfo.PropertyType;
		    if(propertyType == typeof(string))
		    	return new RandomStringGenerator(0, 10);
            if (propertyType == typeof(int))
                return new RandomWholeNumberGenerator(0, 100);            
            if (propertyType == typeof(decimal))
                return new RandomDecimalNumberGenerator(0, 100);
            if (propertyType.IsEnum)
                return new RandomEnumGenerator(propertyType);
            return propertyType == typeof(DateTime) ? new RandomDateTimeGenerator() : null;
		}
	}


}
