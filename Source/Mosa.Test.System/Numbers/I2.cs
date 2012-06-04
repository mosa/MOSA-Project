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
	public static class I2
	{
		private static IList<short> series = null;

		public static IEnumerable<short> Series
		{
			get
			{
				if (series == null) series = GetSeries();

				foreach (short value in series)
					yield return value;
			}
		}

		public static IList<short> GetSeries()
		{
			List<short> list = new List<short>();

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

			// Get negatives
			list.AddIfNew(GetNegatives(list));

			list.Sort();

			return list;
		}

		private static IList<short> GetNegatives(IList<short> list)
		{
			List<short> negs = new List<short>();

			foreach (short value in list)
			{
				if (value > 0)
					negs.AddIfNew<short>((short)-value);
			}

			return negs;
		}
	}
}
