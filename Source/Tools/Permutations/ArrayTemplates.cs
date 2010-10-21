using System;
using System.Collections.Generic;
using System.Text;

namespace Mosa.Tools.Permutations
{
	public class ArrayTemplates
	{
		protected static string Newarr = "public void Newarr{b}()  {  this.arrayTests.Newarr();  }";
		protected static string Ldlen = "public void Ldlen{b}(int length)  {  this.arrayTests.Ldlen(length);  }";
		protected static string Stelem = "public void Stelem{t0}(int index, {0} value)  {  this.arrayTests.Stelem(index, value);  }";
		protected static string Ldelem = "public void Ldelem{t0}(int index, {0} value)  {  this.arrayTests.Ldelem(index, value);  }";
		protected static string Ldelema = "public void Ldelema{t0}(int index, {0} value)  {  this.arrayTests.Ldelema(index, value);  }";

		public static string CreateNewarr(string type, bool isChar)
		{
			return Templates.CreateUnitTest(type, "Newarr", Newarr);
		}

		public static string CreateLdlen(string type, IList<string> a, bool isChar)
		{
			return Templates.CreateUnitTest(type, "Ldlen", Ldlen, new string[] { type }, new IList<string>[] { a });
		}

		public static string CreateStelem(string type, IList<string> a, IList<string> b, bool isChar)
		{
			return Templates.CreateUnitTest(type, "Stelem", Stelem, new string[] { type }, new IList<string>[] { a, Constants.SubstituteWithConstants(b, isChar) });
		}

		public static string CreateLdelem(string type, IList<string> a, IList<string> b, bool isChar)
		{
			return Templates.CreateUnitTest(type, "Ldelem", Ldelem, new string[] { type }, new IList<string>[] { a, Constants.SubstituteWithConstants(b, isChar) });
		}

		public static string CreateLdelema(string type, IList<string> a, IList<string> b, bool isChar)
		{
			return Templates.CreateUnitTest(type, "Ldelema", Ldelema, new string[] { type }, new IList<string>[] { a, Constants.SubstituteWithConstants(b, isChar) });
		}
	}
}
