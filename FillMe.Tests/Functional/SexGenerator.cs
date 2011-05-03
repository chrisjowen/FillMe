using System;
using System.Linq;

namespace FillMe.Tests.Functional
{
    public class SexGenerator : IGenerateDummyData
    {
        public object Generate(object rootObject)
        {
            return Enum.GetValues(typeof(Sex)).Cast<Sex>().ElementAt(new Random().Next(0, 2));
        }
    }
}