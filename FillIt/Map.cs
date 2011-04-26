using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq.Expressions;

namespace FillIt
{
	public class Map
	{
		private IDictionary<Type, object> mapSets = new Dictionary<Type, object>();
		
		public IDictionary<Type, object> MapSets
		{
			get{
				return mapSets;
			}
		}
		
		private IProvideDefaultGenerators generatorFactory = new DefaultGeneratorFactory();
		public IProvideDefaultGenerators DefaultGeneratorFactory {
			get{
				return generatorFactory;
			}
			set
			{
				generatorFactory = value;
			}
		}
		
		public MapSet<T> For<T>(Action<MapSet<T>> setupAction)
		{		
			MapSet<T> mapping;
			Type type = typeof(T);
			
			if(!mapSets.ContainsKey(type)){
				mapping = new MapSet<T>(DefaultGeneratorFactory, this);
				mapSets.Add(type, mapping);
			}
			else{
				mapping = mapSets[type] as MapSet<T>;
			}
				
			setupAction.Invoke(mapping);
			return mapping;
		}	
		
		public void FillMe<T>(T objectToFill)
		{
			new ObjectFiller<T>(objectToFill, mapSets).Fill(objectToFill);
		}
	}

public class ObjectFiller<rootT>
{
	
	private IDictionary<Type, object> mapSets;
	private rootT rootObject;
	
	public ObjectFiller(rootT rootObject, IDictionary<Type, object> mapSets)
	{
		this.mapSets = mapSets;
		this.rootObject = rootObject;
	}
	
	public void Fill<T>(T objectToFill){
		MapSet<T> mapping;
		Type type = typeof(T);
		
		if(mapSets.ContainsKey(type)){
			mapping = mapSets[type] as MapSet<T>;
			FillMeImpl(mapping, objectToFill);
		}
		else
			throw new Exception(string.Format("No mapping found for type {0}", typeof(T)));
	}
	
	private void FillMeImpl<T>(MapSet<T> mapping, T objectToFill)
	{
		foreach(var property in typeof(T).GetProperties(BindingFlags.Instance | BindingFlags.Public)){
			if(property.PropertyType.IsAPrimitive()){
				mapping.For(property).FillMe(objectToFill, rootObject);
			}
			else{
				var childObject = Activator.CreateInstance(property.PropertyType);
				property.SetValue(objectToFill, childObject, null);
				var fillMeMethod = this.GetType().GetMethod("Fill", BindingFlags.Instance | BindingFlags.Public);
				fillMeMethod.MakeGenericMethod(property.PropertyType)
					.Invoke(this,  new object[] { childObject });
			}
		}
	}
}
}
