
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq.Expressions;
namespace FillIt
{
	public static class TypeExtensions{
		public static bool IsAPrimitive(this Type t){
			return t.IsPrimitive || t == typeof(Decimal) || t == typeof(String);
		}
	}
}
