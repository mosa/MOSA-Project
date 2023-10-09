// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Utility.UnitTests.Numbers;

public static class I8
{
	private static IList<long> series;

	public static IEnumerable<long> Series
	{
		get
		{
			if (series == null) series = GetSeries();

			foreach (var value in series)
				yield return value;
		}
	}

	public static IList<long> GetSeries()
	{
		var list = new List<long>
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
			int.MaxValue - 1,
			long.MinValue,
			long.MaxValue,
			long.MinValue + 1,
			long.MaxValue - 1
		};

		AddNegatives(list);
		list = list.Distinct().ToList();
		list.Sort();

		return list;
	}

	private static void AddNegatives(IList<long> list)
	{
		foreach (var value in list.ToArray())
		{
			if (value > 0)
				list.Add(-value);
		}
	}
}
