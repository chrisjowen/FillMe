namespace FillMe.Tests.Functional
{
    public class TestDependentPropertyGenerator : IGenerateDummyData
    {
        public object Generate(object rootObject)
        {
            return ((Foo) rootObject).Age + 10;
        }
    }
}
