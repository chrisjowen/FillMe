using System;
using System.Collections;

namespace FillMe.Extensions
{
	public static class TypeExtensions{
		public static bool IsAStandardType(this Type t){
			return t.IsPrimitive || t == typeof(Decimal) || t == typeof(String) || t == typeof(DateTime);
		}
        public static bool IsEnumerable(this Type t)
        {
            return !t.IsAStandardType() && typeof(IEnumerable).IsAssignableFrom(t);
        }

        public static bool IsAGenericList(this Type t)
        {
            return !t.IsAStandardType() && typeof(IList).IsAssignableFrom(t) && t.IsGenericType;
        }
	}
}
