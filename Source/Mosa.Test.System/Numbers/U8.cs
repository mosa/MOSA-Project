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
	public static class U8
	{
		private static IList<ulong> series = null;

		public static IEnumerable<ulong> Series
		{
			get
			{
				if (series == null) series = GetSeries();

				foreach (ulong value in series)
					yield return value;
			}
		}

		public static IList<ulong> GetSeries()
		{
			List<ulong> list = new List<ulong>();

			list.AddIfNew<ulong>(0);
			list.AddIfNew<ulong>(1);
			list.AddIfNew<ulong>(2);
			list.AddIfNew<ulong>(byte.MinValue);
			list.AddIfNew<ulong>(byte.MaxValue);
			list.AddIfNew<ulong>(byte.MinValue + 1);
			list.AddIfNew<ulong>(byte.MaxValue - 1);
			list.AddIfNew<ulong>(ushort.MinValue);
			list.AddIfNew<ulong>(ushort.MaxValue);
			list.AddIfNew<ulong>(ushort.MinValue + 1);
			list.AddIfNew<ulong>(ushort.MaxValue - 1);
			list.AddIfNew<ulong>(uint.MinValue);
			list.AddIfNew<ulong>(uint.MaxValue);
			list.AddIfNew<ulong>(uint.MinValue + 1);
			list.AddIfNew<ulong>(uint.MaxValue - 1);
			list.AddIfNew<ulong>(ulong.MinValue);
			list.AddIfNew<ulong>(ulong.MaxValue);
			list.AddIfNew<ulong>(ulong.MinValue + 1);
			list.AddIfNew<ulong>(ulong.MaxValue - 1);

			list.Sort();

			return list;
		}

	}
}
