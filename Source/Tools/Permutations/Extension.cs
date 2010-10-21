using System;
using System.Collections.Generic;
using System.Text;

namespace Mosa.Tools.Permutations
{
	public static class Extension
	{
		public static void AddIfNew(this IList<string> list, string value)
		{
			if (!list.Contains(value))
				list.Add(value);
		}

		public static void AddIfNew(this IList<string> list, IList<string> values)
		{
			foreach (string value in values)
				list.AddIfNew(value);
		}
	}
}
