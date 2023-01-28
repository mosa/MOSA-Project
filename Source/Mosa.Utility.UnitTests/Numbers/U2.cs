// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections.Generic;
using System.Linq;

namespace Mosa.Utility.UnitTests.Numbers;

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
		var list = new List<ushort>
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
			ushort.MaxValue - 1
		};

		list = list.Distinct().ToList();
		list.Sort();

		return list;
	}
}
