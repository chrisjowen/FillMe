using System.Reflection;
using FillMe.Exceptions;
using FillMe.Extensions;

namespace FillMe
{
    public class MappingItem : IMappingItem
    {
        private IGenerateDummyData generator;
        public PropertyInfo Property { get; private set; }

        public int ItterationTimes { get; private set; }

        internal int OrderNum;
  
        public MappingItem(PropertyInfo property)
        {
            Property = property;
            ItterationTimes = 1;
        }

        public IMappingItem Use(IGenerateDummyData generator)
        {
            if (Property.PropertyType.IsEnumerable())
                throw new CantAssignGeneratorProblem("Unable to assign generator to enumerable property");
            this.generator = generator;
            return this;
        }

        public IMappingItem Order(int order)
        {
            OrderNum = order;
            return this;
        }

        public IGenerateDummyData Generator
        {
            get { return generator; }
        }


        public IMappingItem Times(int times)
        {
            ItterationTimes = times;
            return this;
        }
    }

    public interface IMappingItem
    {
        IMappingItem Use(IGenerateDummyData generatorOne);
        IGenerateDummyData Generator { get; }
        IMappingItem Order(int order);
        PropertyInfo Property { get; }
        int ItterationTimes { get; }
    }
}
