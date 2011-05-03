
using System;
using System.Text;
using System.Reflection;
namespace FillIt
{
	public interface IGenerateDummyData{
		object Generate(object rootObject);
	}
	
	public interface IGenerateDummyData<T> : IGenerateDummyData{
		T GenerateData(object rootObject);
	}
}
