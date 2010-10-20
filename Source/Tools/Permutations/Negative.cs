using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mosa.Tools.Permutations
{
	static class Negative
	{
		public static IList<string> GetNegatives(long lowest, IList<string> values)
		{
			IList<string> negatives = new List<string>();

			foreach (string value in values)
			{
				if (value.StartsWith("-") || value == "0")
					continue;

				long value2 = -Convert.ToInt64(value);

				if (value2 >= lowest)
					negatives.AddIfNew(value2.ToString());
			}

			return negatives;
		}

		public static IList<string> GetNegatives(IList<string> values)
		{
			IList<string> negatives = new List<string>();

			foreach (string value in values)
			{
				if (value.StartsWith("-") || value == "0")
					continue;

				if (char.IsDigit(value[0]))
					negatives.AddIfNew("-" + value);
			}

			return negatives;
		}
	}
}
