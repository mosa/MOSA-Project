// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common;

namespace Mosa.Utility.UnitTests.Numbers;

public static class U1
{
	private static IList<byte> series;

	public static IEnumerable<byte> Series
	{
		get
		{
			series ??= GetSeries();

			foreach (var value in series)
				yield return value;
		}
	}

	public static IList<byte> GetSeries()
	{
		var list = new List<byte>
		{
			0,
			1,
			2,
			byte.MinValue,
			byte.MaxValue,
			byte.MinValue + 1,
			byte.MaxValue - 1
		};

		list = list.Distinct().ToList();

		//for (var i = 0; i < 8; i++)
		//{
		//	var v = 1 << i;
		//	list.AddIfNew((byte)v);
		//	list.AddIfNew((byte)(v + 1));
		//	list.AddIfNew((byte)(v - 2));
		//}

		list.Sort();
		return list;
	}
}
