// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common;

namespace Mosa.Utility.UnitTests.Numbers;

public static class I4
{
	private static IList<int> series;

	public static IEnumerable<int> Series
	{
		get
		{
			series ??= GetSeries();

			foreach (var value in series)
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

		//for (var i = 0; i < 32; i++)
		//{
		//	var v = 1 << i;
		//	list.AddIfNew(v);
		//	list.AddIfNew(v + 1);
		//	list.AddIfNew(v - 2);
		//}

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
