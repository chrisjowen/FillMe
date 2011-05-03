using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Linq;

namespace FillMe
{
    public class MappingSet<T> : IMappingSet
    {
        private readonly IProvideDefaultGenerators defaultGeneratorFactory;

        internal IDictionary<PropertyInfo, MappingItem> MappingItems 
            = new Dictionary<PropertyInfo, MappingItem>();

        public Type Type { get; private set; }

        public IEnumerable<MappingItem> Items
        {
            get { return MappingItems.Select(m => m.Value).OrderBy(item => item.OrderNum).ToList(); }
        }

        public IMappingItem GetForProperty(PropertyInfo propertyInfo)
        {
            return !MappingItems.ContainsKey(propertyInfo) 
                ? null 
                : MappingItems[propertyInfo];
        }

        public MappingSet()
            : this(new DefaultGeneratorFactory())
        {
            Type = typeof(T);
        }

        public MappingSet(IProvideDefaultGenerators defaultGeneratorFactory)
        {
            this.defaultGeneratorFactory = defaultGeneratorFactory;
        }

        public MappingItem For(Expression<Func<T, object>> memberExpression)
        {
            return For(GetPropertyInfoFrom(memberExpression));
        }

        public IMappingItem DefaultFor(Expression<Func<T, object>> memberExpression)
        {
            var property = GetPropertyInfoFrom(memberExpression);
            return For(property).Use(defaultGeneratorFactory.GetFor(property));
        }

        public MappingItem For(PropertyInfo propertyInfo)
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