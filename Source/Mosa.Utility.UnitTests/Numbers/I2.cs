// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Utility.UnitTests.Numbers;

public static class I2
{
	private static IList<short> series;

	public static IEnumerable<short> Series
	{
		get
		{
			if (series == null) series = GetSeries();

			foreach (var value in series)
				yield return value;
		}
	}

	public static IList<short> GetSeries()
	{
		var list = new List<short>
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
			short.MaxValue - 1
		};

		AddNegatives(list);
		list = list.Distinct().ToList();
		list.Sort();

		return list;
	}

	private static void AddNegatives(IList<short> list)
	{
		foreach (var value in list.ToArray())
		{
			if (value > 0)
				list.Add((short)-value);
		}
	}
}
