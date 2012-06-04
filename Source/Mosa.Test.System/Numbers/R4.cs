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
	public static class R4
	{
		private static IList<float> series = null;

		public static IEnumerable<float> Series
		{
			get
			{
				if (series == null) series = GetSeries();

				foreach (float value in series)
					yield return value;
			}
		}

		public static IList<float> GetSeries()
		{
			List<float> list = new List<float>();

			list.Add(0);
			list.Add(1);
			list.Add(2);
			list.Add(float.MinValue);
			list.Add(float.MaxValue);
			list.Add(float.NaN);
			list.Add(float.PositiveInfinity);
			list.Add(float.NegativeInfinity);
			list.Add(1.00012f);
			list.Add(17.0002501f);
			list.Add(23f);
			list.Add(12321452132.561f);

			// Get negatives
			list.AddIfNew(GetNegatives(list));

			list.Sort();

			return list;
		}

		private static IList<float> GetNegatives(IList<float> list)
		{
			List<float> negs = new List<float>();

			foreach (float value in list)
			{
				if (value > 0)
					negs.AddIfNew<float>((float)-value);
			}

			return negs;
		}
	}
}
