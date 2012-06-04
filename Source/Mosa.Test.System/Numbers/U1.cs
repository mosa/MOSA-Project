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
	public static class U1
	{
		private static IList<byte> series = null;

		public static IEnumerable<byte> Series
		{
			get
			{
				if (series == null) series = GetSeries();

				foreach (byte value in series)
					yield return value;
			}
		}

		public static IList<byte> GetSeries()
		{
			List<byte> list = new List<byte>();

			list.AddIfNew<byte>(0);
			list.AddIfNew<byte>(1);
			list.AddIfNew<byte>(2);
			list.AddIfNew<byte>(byte.MinValue);
			list.AddIfNew<byte>(byte.MaxValue);
			list.AddIfNew<byte>(byte.MinValue + 1);
			list.AddIfNew<byte>(byte.MaxValue - 1);

			list.Sort();

			return list;
		}

	}
}
