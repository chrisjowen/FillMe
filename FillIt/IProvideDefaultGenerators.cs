
using System;
using System.Text;
using System.Reflection;
namespace FillIt
{
	public interface IProvideDefaultGenerators
	{
		IGenerateDummyData GetFor(PropertyInfo propertyInfo);
	}
}
