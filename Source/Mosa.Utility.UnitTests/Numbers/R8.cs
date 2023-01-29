// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections.Generic;
using System.Linq;

namespace Mosa.Utility.UnitTests.Numbers;

public static class R8
{
	private static IList<double> series = null;

	public static IEnumerable<double> Series
	{
		get
		{
			if (series == null) series = GetSeries();

			foreach (double value in series)
				yield return value;
		}
	}

	public static IList<double> GetSeries()
	{
		var list = new List<double>
		{
			0,
			1,
			2,

			//list.Add(double.MinValue);
			//list.Add(double.MaxValue);
			double.NaN,
			double.PositiveInfinity,
			double.NegativeInfinity,
			17,
			23,
			12321452132,

			// negatives
			-1,
			-2,
			-17,
			-23,
			-12321452132
		};

		list = list.Distinct().ToList();
		list.Sort();

		return list;
	}
}
