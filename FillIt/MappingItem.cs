using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq.Expressions;

namespace FillIt
{
	public class MappingItem
	{
		private PropertyInfo propertyInfo;
		public IGenerateDummyData Generator { get; private set;}
		
		public MappingItem(PropertyInfo propertyInfo){
			this.propertyInfo = propertyInfo;
		}
		
		public MappingItem Use(IGenerateDummyData generator)
		{
			Generator = generator;
			return this;
		}
		
		public void FillMe(object objToFill)
		{
			propertyInfo.SetValue(objToFill, Generator.Generate(null), null);
		}
		
		public void FillMe(object objToFill, object rootObject)
		{
			propertyInfo.SetValue(objToFill, Generator.Generate(rootObject), null);
		}
		
	}
}
