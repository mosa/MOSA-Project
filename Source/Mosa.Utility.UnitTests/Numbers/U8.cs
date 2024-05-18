// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common;

namespace Mosa.Utility.UnitTests.Numbers;

public static class U8
{
	private static IList<ulong> series;

	public static IEnumerable<ulong> Series
	{
		get
		{
			series ??= GetSeries();

			foreach (var value in series)
				yield return value;
		}
	}

	public static IList<ulong> GetSeries()
	{
		var list = new List<ulong>
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
			uint.MaxValue - 1,
			ulong.MinValue,
			ulong.MaxValue,
			ulong.MinValue + 1,
			ulong.MaxValue - 1
		};

		list = list.Distinct().ToList();

		//for (var i = 0; i < 64; i++)
		//{
		//	var v = 1ul << i;
		//	list.AddIfNew(v);
		//	list.AddIfNew(v + 1);
		//	list.AddIfNew(v - 2);
		//}

		list.Sort();

		return list;
	}
}
