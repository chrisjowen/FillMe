using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FillMe.Extensions;

namespace FillMe
{
    public class Filler
    {
        private readonly IProvideDefaultGenerators generatorFactory;
        internal IList<IMappingSet> MappingSets = new List<IMappingSet>();

        public Filler(IProvideDefaultGenerators generatorFactory)
        {
            this.generatorFactory = generatorFactory;
        }
        public Filler() : this(new DefaultGeneratorFactory()) { }

        public IMappingSet Configure<T>(Action<MappingSet<T>> mappingAction = null)
        {
            var type = typeof(T);
            if (!MappingSets.Any(s => s.Type == type))
            {
                var mappingSet = new MappingSet<T>(generatorFactory);
                if (mappingAction != null) mappingAction.Invoke(mappingSet);

                MappingSets.Add(mappingSet);
            }

            return FindMappingSetFor(type);
        }

        public T Fill<T>(T rootObject)
        {
            FillImpl(rootObject, rootObject);
            return rootObject;
        }

        private void FillImpl<T>(T objectToFill, object rootObject)
        {
            var type = typeof(T);
            var mappingSet = FindMappingSetFor(type);
            if (mappingSet == null) return;

            foreach (var property in GetOrderedProperties(type, mappingSet))
            {
                PopulateProperty(mappingSet, property, rootObject, objectToFill);
            }
        }

        private void TypedFillImpl(Type type, object objectToFill, object rootObject)
        {
            var fillMeMethod = GetType().GetMethod("FillImpl", BindingFlags.Instance | BindingFlags.NonPublic);
            fillMeMethod.MakeGenericMethod(type).Invoke(this, new[] { objectToFill, rootObject });
        }

        private IEnumerable<PropertyInfo> GetOrderedProperties(Type type, IMappingSet mappingSet)
        {
            var mappedProperties = mappingSet.Items.OrderBy(i => i.OrderNum).Select(m => m.Property);
            var unmappedProperties = GetPropertiesFor(type).Where(p => !mappedProperties.Contains(p));
            return mappedProperties.Concat(unmappedProperties);
        }

        private void PopulateProperty<T>(IMappingSet mappingSet, PropertyInfo property, object rootObject, T objectToFill)
        {
            var mappingItem = mappingSet.GetForProperty(property);

            if (property.PropertyType.IsAStandardType())
            {
                if (mappingItem != null && mappingItem.Generator != null)
                    property.SetValue(objectToFill, mappingItem.Generator.Generate(new GenerationContext(rootObject, objectToFill)), null);
            }
            else
            {
                PopulateObject(mappingItem, property, objectToFill, rootObject);
            }
        }

        private void PopulateObject<T>(IMappingItem mappingItem, PropertyInfo property, T objectToFill, object rootObject)
        {
            var propertyType = property.PropertyType;
            var childObject = CreateInstance(propertyType);
            property.SetValue(objectToFill, childObject, null);

            if (propertyType.IsAGenericList() && mappingItem!=null)
            {
                var listItemType = propertyType.GetGenericArguments().First();
                for(var i = 0; i<mappingItem.ItterationTimes; i++)
                {
                    var listItem = CreateInstance(listItemType);
                    TypedFillImpl(listItemType, listItem, rootObject);
                    ((IList)childObject).Add(listItem);
                }
            }
            else
            {
                TypedFillImpl(propertyType, childObject, rootObject);
            }
        }

        private object CreateInstance(Type type)
        {
            return Activator.CreateInstance(type);
        }

        private IEnumerable<PropertyInfo> GetPropertiesFor(Type type)
        {
            return type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
        }

        private IMappingSet FindMappingSetFor(Type type)
        {
            return MappingSets.FirstOrDefault(set => set.Type == type);
        }

  
    }
}