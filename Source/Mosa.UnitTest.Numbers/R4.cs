// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections.Generic;
using System.Linq;

namespace Mosa.UnitTest.Numbers
{
	public static class R4
	{
		private static IList<float> series = null;

		public static IEnumerable<float> Series
		{
			get
			{
				if (series == null) series = GetSeries();

				foreach (float value in series)
					yield return value;
			}
		}

		public static IList<float> GetSeries()
		{
			var list = new List<float>
			{
				0,
				1,
				2,
				float.MinValue,
				float.MaxValue,
				float.NaN,
				float.PositiveInfinity,
				float.NegativeInfinity,
				1.00012f,
				17.0002501f,
				23f,
				12321452132.561f,

				// negatives
				-1,
				-2,
				-1.00012f,
				-17.0002501f,
				-23f,
				-12321452132.561f
			};

			list = list.Distinct().ToList();
			list.Sort();

			return list;
		}

		private static IList<float> GetNegatives(IList<float> list)
		{
			var negs = new List<float>();

			foreach (float value in list)
			{
				if (value > 0)
					negs.AddIfNew<float>(-value);
			}

			return negs;
		}
	}
}
