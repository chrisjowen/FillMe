using System;
using System.Collections.Generic;
using System.Reflection;

namespace FillMe
{
    public interface IMappingSet
    {
        Type Type { get; }
        IEnumerable<IMappingItem> Items { get; }
        IMappingItem GetForProperty(PropertyInfo property);
        IMappingSet Defaults();
    }
}