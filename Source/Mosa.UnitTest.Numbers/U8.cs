// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections.Generic;
using System.Linq;

namespace Mosa.UnitTest.Numbers
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
			var list = new List<ulong>
			{
				0,
				1,
				2,
				byte.MinValue,
				byte.MaxValue,
				byte.MinValue + 1,
				byte.MaxValue - 1,
				ushort.MinValue,
				ushort.MaxValue,
				ushort.MinValue + 1,
				ushort.MaxValue - 1,
				uint.MinValue,
				uint.MaxValue,
				uint.MinValue + 1,
				uint.MaxValue - 1,
				ulong.MinValue,
				ulong.MaxValue,
				ulong.MinValue + 1,
				ulong.MaxValue - 1
			};

			list = list.Distinct().ToList();
			list.Sort();

			return list;
		}
	}
}
