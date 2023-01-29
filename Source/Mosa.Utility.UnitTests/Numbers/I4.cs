// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections.Generic;
using System.Linq;

namespace Mosa.Utility.UnitTests.Numbers;

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
		var list = new List<int>
		{
			0,
			1,
			2,
			sbyte.MinValue,
			sbyte.MaxValue,
			sbyte.MinValue + 1,
			sbyte.MaxValue - 1,
			byte.MaxValue,
			byte.MinValue,
			short.MinValue,
			short.MaxValue,
			short.MinValue + 1,
			short.MaxValue - 1,
			int.MinValue,
			int.MaxValue,
			int.MinValue + 1,
			int.MaxValue - 1
		};

		// Add negatives
		AddNegatives(list);

		list = list.Distinct().ToList();
		list.Sort();

		return list;
	}

	private static void AddNegatives(IList<int> list)
	{
		foreach (var value in list.ToArray())
		{
			if (value > 0)
				list.Add(-value);
		}
	}
}
