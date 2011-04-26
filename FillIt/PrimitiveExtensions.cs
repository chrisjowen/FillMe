
using System;
using System.Text;
using System.Reflection;
namespace FillIt
{
	public static class PrimitiveExtensions{
		public static void Times(this int times, Action action){
			for(var i=0; i<times; i++)
				action();
		}
		
		public static char RandChar(this string s){
			return s[new Random().Next(0, s.Length)];
		}
	}
}
