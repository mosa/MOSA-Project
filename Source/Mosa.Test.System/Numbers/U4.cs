/*
* (c) 2008 MOSA - The Managed Operating System Alliance
*
* Licensed under the terms of the New BSD License.
*
* Authors:
*  Phil Garcia (tgiphil) <phil@thinkedge.com>
*/

using System.Collections.Generic;


namespace Mosa.Test.System.Numbers
{
	public static class U4
	{
		private static IList<uint> series = null;

		public static IEnumerable<uint> Series
		{
			get
			{
				if (series == null) series = GetSeries();

				foreach (uint value in series)
					yield return value;
			}
		}

		public static IList<uint> GetSeries()
		{
			List<uint> list = new List<uint>();

			list.AddIfNew<uint>(0);
			list.AddIfNew<uint>(1);
			list.AddIfNew<uint>(2);
			list.AddIfNew<uint>(byte.MinValue);
			list.AddIfNew<uint>(byte.MaxValue);
			list.AddIfNew<uint>(byte.MinValue + 1);
			list.AddIfNew<uint>(byte.MaxValue - 1);
			list.AddIfNew<uint>(ushort.MinValue);
			list.AddIfNew<uint>(ushort.MaxValue);
			list.AddIfNew<uint>(ushort.MinValue + 1);
			list.AddIfNew<uint>(ushort.MaxValue - 1);
			list.AddIfNew<uint>(uint.MinValue);
			list.AddIfNew<uint>(uint.MaxValue);
			list.AddIfNew<uint>(uint.MinValue + 1);
			list.AddIfNew<uint>(uint.MaxValue - 1);

			list.Sort();

			return list;
		}

	}
}
