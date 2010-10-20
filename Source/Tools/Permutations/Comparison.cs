using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mosa.Tools.Permutations
{
	static class Comparison
	{
		public static IList<string> GetComparisons(bool includeNotEqual)
		{
			IList<string> comparisons = new List<string>();

			comparisons.Add("==");
			comparisons.Add(">");
			comparisons.Add("<");
			comparisons.Add("=>");
			comparisons.Add("<=");
			if (includeNotEqual)
				comparisons.Add("!=");

			return comparisons;
		}

		public static string ToString(string op)
		{
			switch (op)
			{
				case "==": return "Ceq";
				case ">": return "Cgt";
				case "<": return "Clt";
				case "=>": return "Cge";
				case "<=": return "Cle";
				case "!=": return "NotCeq";
				//case "==": return "EQ";
				//case "!=": return "NEQ";
				//case ">": return "GT";
				//case "<": return "LT";
				//case "=>": return "GTE";
				//case "<=": return "LTE";
				default: return string.Empty;
			}
		}
	}
}
