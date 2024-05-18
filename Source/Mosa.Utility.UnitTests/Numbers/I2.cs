// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common;

namespace Mosa.Utility.UnitTests.Numbers;

public static class I2
{
	private static IList<short> series;

	public static IEnumerable<short> Series
	{
		get
		{
			series ??= GetSeries();

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

		//for (var i = 0; i < 16; i++)
		//{
		//	var v = 1 << i;
		//	list.AddIfNew((short)v);
		//	list.AddIfNew((short)(v + 1));
		//	list.AddIfNew((short)(v - 2));
		//}

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
