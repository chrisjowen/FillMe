using System.Reflection;

namespace FillMe
{
    public interface IProvideDefaultGenerators
    {
        IGenerateDummyData GetFor(PropertyInfo propertyInfo);
    }
}