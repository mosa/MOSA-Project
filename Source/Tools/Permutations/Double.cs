using System;
using System.Collections.Generic;
using System.Text;

namespace Mosa.Tools.Permutations
{
	public static class Double
	{
		public static string Type = "double";

		private static IList<string> allPermutations = null;
		public static IList<string> AllPermutations { get { if (allPermutations == null) allPermutations = GetAllPermutations(); return allPermutations; } }

		public static IList<string> GetAllPermutations()
		{
			IList<string> list = new List<string>();

			list.AddIfNew(0.ToString());
			list.AddIfNew(1.ToString());
			list.AddIfNew(2.ToString());
			list.AddIfNew(double.MinValue.ToString());
			list.AddIfNew(double.MaxValue.ToString());
			list.AddIfNew("double.NaN");
			list.AddIfNew("double.PositiveInfinity");
			list.AddIfNew("double.NegativeInfinity");

			list.AddIfNew(1.00012.ToString());
			//list.AddIfNew(1.045.ToString());
			list.AddIfNew(1.1497.ToString());
			list.AddIfNew(1.2.ToString());
			//list.AddIfNew(1.5.ToString());
			//list.AddIfNew(1.67.ToString());
			//list.AddIfNew(1.78.ToString());
			//list.AddIfNew(2.1.ToString());
			//list.AddIfNew(2.11.ToString());
			//list.AddIfNew(2.198.ToString());
			//list.AddIfNew(2.3.ToString());
			list.AddIfNew(17.0002501.ToString());
			//list.AddIfNew(17.094002.ToString());
			//list.AddIfNew(17.1.ToString());
			//list.AddIfNew(17.12.ToString());
			//list.AddIfNew(17.4.ToString());
			//list.AddIfNew(17.59.ToString());
			//list.AddIfNew(17.6.ToString());
			//list.AddIfNew(17.875.ToString());
			//list.AddIfNew(17.99.ToString());
			//list.AddIfNew(21.2578.ToString());
			list.AddIfNew(23.ToString());
			//list.AddIfNew(123.001.ToString());
			//list.AddIfNew(123.023.ToString());
			//list.AddIfNew(123.1.ToString());
			//list.AddIfNew(123.235.ToString());
			//list.AddIfNew(123.283.ToString());
			//list.AddIfNew(123.34.ToString());
			//list.AddIfNew(123.41.ToString());
			list.AddIfNew(12321452132.561.ToString());

			// Get negatives
			list.AddIfNew(Negative.GetNegatives(list));

			return list;
		}

		public static IList<string> GetPermutationResults()
		{
			List<string> results = new List<string>();

			results.Add(NumericTemplates.CreateAdd(Type, AllPermutations, AllPermutations, false));
			results.Add(NumericTemplates.CreateSub(Type, AllPermutations, AllPermutations, false));
			results.Add(NumericTemplates.CreateMul(Type, AllPermutations, AllPermutations, false));
			results.Add(NumericTemplates.CreateDiv(Type, AllPermutations, AllPermutations, false, false));

			//results.Add(NumericTemplates.CreateRem(Type, AllPermutations, AllPermutations, false));
			results.Add(NumericTemplates.CreateNeg(Type, AllPermutations, false));
			results.Add(NumericTemplates.CreateRet(Type, AllPermutations, false));

			results.Add(NumericTemplates.CreateCeq(Type, AllPermutations, AllPermutations, false));
			results.Add(NumericTemplates.CreateCgt(Type, AllPermutations, AllPermutations, false));
			results.Add(NumericTemplates.CreateClt(Type, AllPermutations, AllPermutations, false));
			results.Add(NumericTemplates.CreateCge(Type, AllPermutations, AllPermutations, false));
			results.Add(NumericTemplates.CreateCle(Type, AllPermutations, AllPermutations, false));

			return results;
		}
	}
}
