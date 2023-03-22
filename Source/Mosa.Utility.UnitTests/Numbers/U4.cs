// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections.Generic;
using System.Linq;

namespace Mosa.Utility.UnitTests.Numbers;

public static class U4
{
	private static IList<uint> series;

	public static IEnumerable<uint> Series
	{
		get
		{
			if (series == null) series = GetSeries();

			foreach (var value in series)
				yield return value;
		}
	}

	public static IList<uint> GetSeries()
	{
		var list = new List<uint>
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
			uint.MaxValue - 1
		};

		list = list.Distinct().ToList();
		list.Sort();

		return list;
	}
}
