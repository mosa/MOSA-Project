using System;
using System.Collections.Generic;
using System.Text;

namespace Mosa.Tools.Permutations
{
	static class Power2
	{

		public static IList<string> GetPowerTwos(ulong min, ulong max, bool includeZero)
		{
			IList<string> list = new List<string>();

			if (includeZero)
				list.Add("0");

			ulong a = 1;

			while (a < max)
			{
				if ((a >= min) && (a <= max))
					list.Add(a.ToString());

				a = a * 2;
			}

			return list;
		}

	}
}
