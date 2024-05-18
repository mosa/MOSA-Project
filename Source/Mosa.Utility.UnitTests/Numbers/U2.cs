// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common;

namespace Mosa.Utility.UnitTests.Numbers;

public static class U2
{
	private static IList<ushort> series;

	public static IEnumerable<ushort> Series
	{
		get
		{
			series ??= GetSeries();

			foreach (var value in series)
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

		//for (var i = 0; i < 16; i++)
		//{
		//	var v = 1 << i;
		//	list.AddIfNew((ushort)v);
		//	list.AddIfNew((ushort)(v + 1));
		//	list.AddIfNew((ushort)(v - 2));
		//}

		list.Sort();

		return list;
	}
}
