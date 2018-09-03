// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections.Generic;
using System.Linq;

namespace Mosa.Utility.UnitTests.Numbers
{
	public static class U1
	{
		private static IList<byte> series = null;

		public static IEnumerable<byte> Series
		{
			get
			{
				if (series == null) series = GetSeries();

				foreach (byte value in series)
					yield return value;
			}
		}

		public static IList<byte> GetSeries()
		{
			var list = new List<byte>
			{
				0,
				1,
				2,
				byte.MinValue,
				byte.MaxValue,
				byte.MinValue + 1,
				byte.MaxValue - 1
			};

			list = list.Distinct().ToList();
			list.Sort();

			return list;
		}
	}
}
