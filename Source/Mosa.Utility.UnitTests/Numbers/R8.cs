// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections.Generic;
using System.Linq;

namespace Mosa.Utility.UnitTests.Numbers;

public static class R8
{
	private static IList<double> series;

	public static IEnumerable<double> Series
	{
		get
		{
			if (series == null) series = GetSeries();

			foreach (var value in series)
				yield return value;
		}
	}

	public static IList<double> GetSeries()
	{
		var list = new List<double>
		{
			0d,
			1d,
			2d,
			double.MinValue,
			double.MaxValue,
			double.NaN,
			double.PositiveInfinity,
			double.NegativeInfinity,
			1.00012d,
			17.0002501d,
			23d,
			12321452132.561d,
			17d,
			12321452132d,
			// negatives
			-1d,
			-2d,
			-17d,
			-23d,
			-12321452132d
			-1.00012d,
			-17.0002501d,
			-12321452132.561d
		};

		list = list.Distinct().ToList();
		list.Sort();

		return list;
	}
}
