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
	public static class I4
	{
		private static IList<int> series = null;

		public static IEnumerable<int> Series
		{
			get
			{
				if (series == null) series = GetSeries();

				foreach (int value in series)
					yield return value;
			}
		}

		public static IList<int> GetSeries()
		{
			List<int> list = new List<int>();

			list.Add(0);
			list.Add(1);
			list.Add(2);
			list.Add(sbyte.MinValue);
			list.Add(sbyte.MaxValue);
			list.Add(sbyte.MinValue + 1);
			list.Add(sbyte.MaxValue - 1);
			list.Add(byte.MaxValue);
			list.Add(byte.MinValue);
			list.Add(short.MinValue);
			list.Add(short.MaxValue);
			list.Add(short.MinValue + 1);
			list.Add(short.MaxValue - 1);
			list.Add(int.MinValue);
			list.Add(int.MaxValue);
			list.Add(int.MinValue + 1);
			list.Add(int.MaxValue - 1);

			// Get negatives
			list.AddIfNew(GetNegatives(list));

			list.Sort();

			return list;
		}

		private static IList<int> GetNegatives(IList<int> list)
		{
			List<int> negs = new List<int>();

			foreach (int value in list)
			{
				if (value > 0)
					negs.AddIfNew((int)-value);
			}

			return negs;
		}
	}
}
