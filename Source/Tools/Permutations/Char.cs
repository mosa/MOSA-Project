using System;
using System.Collections.Generic;
using System.Text;

namespace Mosa.Tools.Permutations
{
	public static class Char
	{
		public static string Type = "char";
		public static byte Bits = 16;

		private static IList<string> allPermutations = null;
		public static IList<string> AllPermutations { get { if (allPermutations == null) allPermutations = GetAllPermutations(); return allPermutations; } }

		public static IList<string> GetAllPermutations()
		{
			IList<string> list = new List<string>();

			list.AddIfNew(((int)char.MinValue).ToString());
			list.AddIfNew(((int)char.MaxValue).ToString());
			list.AddIfNew(0.ToString());
			list.AddIfNew(1.ToString());
			list.AddIfNew(13.ToString());
			list.AddIfNew("'0'");
			list.AddIfNew("'9'");
			list.AddIfNew("'A'");
			list.AddIfNew("'Z'");
			list.AddIfNew("'a'");
			list.AddIfNew("'z'");
			list.AddIfNew("' '");
			list.AddIfNew(@"'\n'");
			list.AddIfNew(@"'\t'");
			list.AddIfNew(127.ToString());
			list.AddIfNew(128.ToString());
			list.AddIfNew(255.ToString());
			list.AddIfNew(256.ToString());

			return list;
		}

		public static IList<string> GetPermutationResults()
		{
			List<string> results = new List<string>();

			results.Add(NumericTemplates.CreateAdd(Type, AllPermutations, AllPermutations, true));
			results.Add(NumericTemplates.CreateSub(Type, AllPermutations, AllPermutations, true));
			results.Add(NumericTemplates.CreateMul(Type, AllPermutations, AllPermutations, true));
			results.Add(NumericTemplates.CreateDiv(Type, AllPermutations, AllPermutations, true, true));

			results.Add(NumericTemplates.CreateRem(Type, AllPermutations, AllPermutations, true));
			results.Add(NumericTemplates.CreateRet(Type, AllPermutations, true));

			results.Add(NumericTemplates.CreateAnd(Type, AllPermutations, AllPermutations, true));
			results.Add(NumericTemplates.CreateOr(Type, AllPermutations, AllPermutations, true));
			results.Add(NumericTemplates.CreateXor(Type, AllPermutations, AllPermutations, true));

			results.Add(NumericTemplates.CreateShl(Type, AllPermutations, Upto.GetUpto(Bits), true));
			results.Add(NumericTemplates.CreateShr(Type, AllPermutations, Upto.GetUpto(Bits), true));

			results.Add(NumericTemplates.CreateCeq(Type, AllPermutations, AllPermutations, true));
			results.Add(NumericTemplates.CreateCgt(Type, AllPermutations, AllPermutations, true));
			results.Add(NumericTemplates.CreateClt(Type, AllPermutations, AllPermutations, true));
			results.Add(NumericTemplates.CreateCge(Type, AllPermutations, AllPermutations, true));
			results.Add(NumericTemplates.CreateCle(Type, AllPermutations, AllPermutations, true));


			return results;
		}
	}
}
