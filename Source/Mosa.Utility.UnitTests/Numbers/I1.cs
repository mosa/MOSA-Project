// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Utility.UnitTests.Numbers;

public static class I1
{
	private static IList<sbyte> series;

	public static IEnumerable<sbyte> Series
	{
		get
		{
			if (series == null) series = GetSeries();

			foreach (var value in series)
				yield return value;
		}
	}

	public static IList<sbyte> GetSeries()
	{
		var list = new List<sbyte>
		{
			0,
			1,
			2,
			sbyte.MinValue,
			sbyte.MaxValue,
			sbyte.MinValue + 1,
			sbyte.MaxValue - 1
		};

		AddNegatives(list);

		list = list.Distinct().ToList();
		list.Sort();

		return list;
	}

	private static void AddNegatives(IList<sbyte> list)
	{
		foreach (var value in list.ToArray())
		{
			if (value > 0)
				list.Add((sbyte)-value);
		}
	}
}
