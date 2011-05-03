using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq.Expressions;

namespace FillIt
{
	public class MapSet<T>
	{
		
		private IDictionary<PropertyInfo, MappingItem> mappingItems 
			= new Dictionary<PropertyInfo, MappingItem>();
		
		private IProvideDefaultGenerators generatorFactory;
		private Map map;
		
		public MapSet(IProvideDefaultGenerators generatorFactory, Map map)
		{
			this.generatorFactory = generatorFactory;
			this.map = map;
		}
		
		public IDictionary<PropertyInfo, MappingItem> MappingItems {
			get{
				return mappingItems;
			}
		}
		
		Type forType{ get{ return typeof(T); } }
		
		public MappingItem For(Expression<Func<T, object>> propertyExpression)
		{
			return For(GetPropertyInfoFrom(propertyExpression));
		}
		
		internal MappingItem For(PropertyInfo propertyInfo)
		{
			if(!mappingItems.ContainsKey(propertyInfo)) 
				mappingItems.Add(propertyInfo, new MappingItem(propertyInfo));
			
			
			return mappingItems[propertyInfo];
		}
		
		private PropertyInfo GetPropertyInfoFrom(Expression<Func<T, object>> propertyExpression)
		{
			var memberExpression = propertyExpression.Body as MemberExpression;
			var propertyInfo = memberExpression.Member as PropertyInfo;
			return propertyInfo;
		}
		
		public void Defaults()
		{
			foreach(var property in typeof(T).GetProperties(BindingFlags.Instance | BindingFlags.Public)){
				var item = For(property);
				if(item.Generator!=null) continue;
				
				if(property.PropertyType.IsAPrimitive())
					item.Use(generatorFactory.GetFor(property));
				else{
					var createMeth = this.GetType().GetMethod("CreateAction", BindingFlags.Static | BindingFlags.NonPublic);
					var l = createMeth.MakeGenericMethod(property.PropertyType).Invoke(this, null);
				    typeof(Map).GetMethod("For").MakeGenericMethod(property.PropertyType).Invoke(map, new object[] { l });
				}
			}
		}
		
		protected static Action<MapSet<actionT>> CreateAction<actionT>() { 
    		return action => action.Defaults();
  		}
		
	}
}

