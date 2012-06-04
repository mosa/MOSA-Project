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
	public static class U2
	{
		private static IList<ushort> series = null;

		public static IEnumerable<ushort> Series
		{
			get
			{
				if (series == null) series = GetSeries();

				foreach (ushort value in series)
					yield return value;
			}
		}

		public static IList<ushort> GetSeries()
		{
			List<ushort> list = new List<ushort>();

			list.AddIfNew<ushort>(0);
			list.AddIfNew<ushort>(1);
			list.AddIfNew<ushort>(2);
			list.AddIfNew<ushort>(byte.MinValue);
			list.AddIfNew<ushort>(byte.MaxValue);
			list.AddIfNew<ushort>(byte.MinValue + 1);
			list.AddIfNew<ushort>(byte.MaxValue - 1);
			list.AddIfNew<ushort>(ushort.MinValue);
			list.AddIfNew<ushort>(ushort.MaxValue);
			list.AddIfNew<ushort>(ushort.MinValue + 1);
			list.AddIfNew<ushort>(ushort.MaxValue - 1);

			list.Sort();

			return list;
		}

	}
}
