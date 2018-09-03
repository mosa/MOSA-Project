// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections.Generic;

namespace Mosa.Utility.UnitTests.Numbers
{
	public static class C
	{
		private static IList<char> series = null;

		public static IEnumerable<char> Series
		{
			get
			{
				if (series == null) series = GetSeries();

				foreach (char value in series)
					yield return value;
			}
		}

		public static IList<char> GetSeries()
		{
			var list = new List<char>
			{
				(char)1,
				(char)2,
				(char)127,
				char.MinValue,
				char.MaxValue,
				'0',
				'9',
				'A',
				'Z',
				'a',
				'z',
				' ',
				'\n',
				'\t'
			};

			list.Sort();

			return list;
		}
	}
}
