/*
* (c) 2008 MOSA - The Managed Operating System Alliance
*
* Licensed under the terms of the New BSD License.
*
* Authors:
*  Phil Garcia (tgiphil) <phil@thinkedge.com>
*/

using System.Collections.Generic;


namespace Mosa.Test.System.Numbers
{
	public static class I1
	{
		private static IList<sbyte> series = null;

		public static IEnumerable<sbyte> Series
		{
			get
			{
				if (series == null) series = GetSeries();

				foreach (sbyte value in series)
					yield return value;
			}
		}

		public static IList<sbyte> GetSeries()
		{
			List<sbyte> list = new List<sbyte>();

			list.Add(0);
			list.Add(1);
			list.Add(2);
			list.Add(sbyte.MinValue);
			list.Add(sbyte.MaxValue);
			list.Add(sbyte.MinValue + 1);
			list.Add(sbyte.MaxValue - 1);

			// Get negatives
			list.AddIfNew(GetNegatives(list));

			list.Sort();

			return list;
		}

		private static IList<sbyte> GetNegatives(IList<sbyte> list)
		{
			List<sbyte> negs = new List<sbyte>();

			foreach (sbyte value in list)
			{
				if (value > 0)
					negs.AddIfNew<sbyte>((sbyte)-value);
			}

			return negs;
		}
	}
}
