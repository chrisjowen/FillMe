using System;
using System.Collections;
using System.Collections.Generic;

namespace FillMe.Extensions
{
	public static class TypeExtensions{
		public static bool IsAPrimitive(this Type t){
			return t.IsPrimitive || t == typeof(Decimal) || t == typeof(String);
		}
        public static bool IsEnumerable(this Type t)
        {
            return !t.IsAPrimitive() && typeof(IEnumerable).IsAssignableFrom(t);
        }

        public static bool IsAGenericList(this Type t)
        {
            return !t.IsAPrimitive() && typeof(IList).IsAssignableFrom(t) && t.IsGenericType;
        }
	}
}
