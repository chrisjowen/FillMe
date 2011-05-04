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
        public bool ShouldIgnore { get; private set; }

  
        public MappingItem(PropertyInfo property)
        {
            Property = property;
            ItterationTimes = 1;
            ShouldIgnore = false;
        }

        public IMappingItem Ignore()
        {
            ShouldIgnore = true;
            return this;
        }
        public IMappingItem Use(IGenerateDummyData generator)
        {
            if (Property.PropertyType.IsEnumerable() && generator!=null)
                throw new CantAssignGeneratorProblem("Unable to assign generator to enumerable property");
            this.generator = generator;
            return this;
        }

        public IMappingItem Do(Func<GenerationContext, object> generateFunc)
        {
            return Use(new GeneratWithFunc(generateFunc));
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

    public class GeneratWithFunc : IGenerateDummyData
    {
        private readonly Func<GenerationContext, object> generateFunc;

        public GeneratWithFunc(Func<GenerationContext, object> generateFunc)
        {
            this.generateFunc = generateFunc;
        }

        public object Generate(GenerationContext context)
        {
            return generateFunc(context);
        }
    }

    public interface IMappingItem
    {
        IMappingItem Use(IGenerateDummyData generatorOne);
        IMappingItem Do(Func<GenerationContext, object> generateFunc);
        IGenerateDummyData Generator { get; }
        IMappingItem Order(int order);
        IMappingItem Times(int times);
        IMappingItem Ignore();
        PropertyInfo Property { get; }
        int ItterationTimes { get; }
        int OrderNum { get; }
        bool ShouldIgnore { get; }

    }
}
