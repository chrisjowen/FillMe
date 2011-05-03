using System;
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
        public int OrderNum { get; private set; }

  
        public MappingItem(PropertyInfo property)
        {
            Property = property;
            ItterationTimes = 1;
        }

        public IMappingItem Use(IGenerateDummyData generator)
        {
            if (Property.PropertyType.IsEnumerable() && generator!=null)
                throw new CantAssignGeneratorProblem("Unable to assign generator to enumerable property");
            this.generator = generator;
            return this;
        }

        public IMappingItem Do<rootT, returnT>(Func<rootT, returnT> generateFunc)
        {
            return Use(new GeneratWithFunc<rootT, returnT>(generateFunc));
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

    public class GeneratWithFunc<rootT, returnT> : IGenerateDummyData
    {
        private readonly Func<rootT, returnT> generateFunc;

        public GeneratWithFunc(Func<rootT, returnT> generateFunc)
        {
            this.generateFunc = generateFunc;
        }

        public object Generate(object rootObject)
        {
            return generateFunc((rootT) rootObject);
        }
    }

    public interface IMappingItem
    {
        IMappingItem Use(IGenerateDummyData generatorOne);
        IMappingItem Do<rootT, returnT>(Func<rootT, returnT> generateFunc);
        IGenerateDummyData Generator { get; }
        IMappingItem Order(int order);
        IMappingItem Times(int times);
        PropertyInfo Property { get; }
        int ItterationTimes { get; }
        int OrderNum { get; }
    }
}
