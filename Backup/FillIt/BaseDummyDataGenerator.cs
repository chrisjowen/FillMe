using System;
using System.Text;
using System.Reflection;

namespace FillIt
{
	public abstract class BaseDummyDataGenerator<T> : IGenerateDummyData, IGenerateDummyData<T>
	{
		public abstract object Generate(object rootObject);
		public T GenerateData(object rootObject)
		{
			return (T)Generate(rootObject);
		}
	}
}
