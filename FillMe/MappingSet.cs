using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Linq;
using FillMe.Extensions;

namespace FillMe
{
    public class MappingSet<T> : IMappingSet
    {
        private readonly IProvideDefaultGenerators defaultGeneratorFactory;

        internal IDictionary<PropertyInfo, IMappingItem> MappingItems 
            = new Dictionary<PropertyInfo, IMappingItem>();

        public Type Type { get; private set; }

        public IEnumerable<IMappingItem> Items
        {
            get { return MappingItems.Select(m => m.Value).OrderBy(item => item.OrderNum).ToList(); }
        }

        public IMappingItem GetForProperty(PropertyInfo propertyInfo)
        {
            return !MappingItems.ContainsKey(propertyInfo) 
                ? null 
                : MappingItems[propertyInfo];
        }


        public IMappingSet UseDefaults()
        {
            var standardUnassignedProperties 
                = GetPropertiesFor(Type).Where(p => p.PropertyType.IsAStandardType() && !MappingItems.ContainsKey(p));
            
            foreach (var property in standardUnassignedProperties){
                MappingItems.Add(property, new MappingItem(property).Use(defaultGeneratorFactory.GetFor(property)));
            }
            return this;
        }

        private static IEnumerable<PropertyInfo> GetPropertiesFor(Type type)
        {
            return type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
        }

        public MappingSet()
            : this(new DefaultGeneratorFactory())
        {
        }

        public MappingSet(IProvideDefaultGenerators defaultGeneratorFactory)
        {
            this.defaultGeneratorFactory = defaultGeneratorFactory;
            Type = typeof(T);
        }

        public IMappingItem For(Expression<Func<T, object>> memberExpression)
        {
            return For(GetPropertyInfoFrom(memberExpression));
        }

        public IMappingItem DefaultFor(Expression<Func<T, object>> memberExpression)
        {
            var property = GetPropertyInfoFrom(memberExpression);
            return For(property).Use(defaultGeneratorFactory.GetFor(property));
        }

        public IMappingItem For(PropertyInfo propertyInfo)
        {
            if (!MappingItems.ContainsKey(propertyInfo))
                MappingItems.Add(propertyInfo, new MappingItem(propertyInfo));

            return MappingItems[propertyInfo];
        }

        private static PropertyInfo GetPropertyInfoFrom(Expression<Func<T, object>> expression)
        {
            var body = (expression.Body.NodeType == ExpressionType.Convert) 
                ? (MemberExpression)((UnaryExpression)expression.Body).Operand 
                : (MemberExpression)expression.Body;
            return body.Member as PropertyInfo;
        }


    }
}