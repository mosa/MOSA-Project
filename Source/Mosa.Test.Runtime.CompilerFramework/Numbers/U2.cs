/*
* (c) 2008 MOSA - The Managed Operating System Alliance
*
* Licensed under the terms of the New BSD License.
*
* Authors:
*  Phil Garcia (tgiphil) <phil@thinkedge.com>
*/

using System;
using System.Collections.Generic;
using System.Text;

using MbUnit.Framework;

namespace Mosa.Test.System.Numbers
{
	public static class U2
	{
		private static IList<ushort> series = null;

		public static IEnumerable<ushort> Series
		{
			get
			{
				if (series == null) series = GetSeries();

				foreach (ushort value in series)
					yield return value;
			}
		}

		public static IList<ushort> GetSeries()
		{
			List<ushort> list = new List<ushort>();

			list.Add(0);
			list.Add(1);
			list.Add(2);
			list.Add(byte.MinValue);
			list.Add(byte.MaxValue);
			list.Add(byte.MinValue + 1);
			list.Add(byte.MaxValue - 1);
			list.Add(ushort.MinValue);
			list.Add(ushort.MaxValue);
			list.Add(ushort.MinValue + 1);
			list.Add(ushort.MaxValue - 1);

			list.Sort();

			return list;
		}

	}
}
