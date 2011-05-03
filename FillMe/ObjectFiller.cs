//using System;
//using System.Collections.Generic;
//using System.Reflection;
//using FillMe;
//using FillMe.Extensions;
//
//namespace FillIt
//{
//    public class ObjectFiller<rootT>
//    {
//
//        private IList<MappingItem> mappingItems;
//        private rootT rootObject;
//
//        public ObjectFiller(rootT rootObject, IList<MappingItem> mappingItems)
//        {
//            this.rootObject = rootObject;
//            this.mappingItems = mappingItems;
//        }
//
//        public void Fill<T>(T objectToFill)
//        {
//            MapSet<T> mapping;
//            Type type = typeof(T);
//
//            if (mapSets.ContainsKey(type))
//            {
//                mapping = mapSets[type] as MapSet<T>;
//                FillMeImpl(mapping, objectToFill);
//            }
//            else
//                throw new Exception(string.Format("No mapping found for type {0}", typeof(T)));
//        }
//
//        private void FillMeImpl<T>(MapSet<T> mapping, T objectToFill)
//        {
//            foreach (var property in typeof(T).GetProperties(BindingFlags.Instance | BindingFlags.Public))
//            {
//                if (property.PropertyType.IsAPrimitive())
//                {
//                    mapping.For(property).FillMe(objectToFill, rootObject);
//                }
//                else
//                {
//                    var childObject = Activator.CreateInstance(property.PropertyType);
//                    property.SetValue(objectToFill, childObject, null);
//                    var fillMeMethod = this.GetType().GetMethod("Fill", BindingFlags.Instance | BindingFlags.Public);
//                    fillMeMethod.MakeGenericMethod(property.PropertyType)
//                        .Invoke(this, new object[] { childObject });
//                }
//            }
//        }
//    }
//}