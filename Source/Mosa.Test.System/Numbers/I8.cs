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
	public static class I8
	{
		private static IList<long> series = null;

		public static IEnumerable<long> Series
		{
			get
			{
				if (series == null) series = GetSeries();

				foreach (long value in series)
					yield return value;
			}
		}

		public static IList<long> GetSeries()
		{
			List<long> list = new List<long>();

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
			list.Add(long.MinValue);
			list.Add(long.MaxValue);
			list.Add(long.MinValue + 1);
			list.Add(long.MaxValue - 1);

			// Get negatives
			list.AddIfNew(GetNegatives(list));

			list.Sort();

			return list;
		}

		private static IList<long> GetNegatives(IList<long> list)
		{
			List<long> negs = new List<long>();

			foreach (long value in list)
			{
				if (value > 0)
					negs.AddIfNew<long>((long)-value);
			}

			return negs;
		}
	}
}
