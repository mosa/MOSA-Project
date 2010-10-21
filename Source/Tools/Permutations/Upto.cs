using System;
using System.Collections.Generic;
using System.Text;

namespace Mosa.Tools.Permutations
{
	static class Upto
	{

		public static IList<string> GetUpto(byte end)
		{
			return GetUpto(0, end);
		}

		public static IList<string> GetUpto(byte start, byte end)
		{
			IList<string> list = new List<string>();

			for (byte i = start; i < end; i++)
				list.Add(i.ToString());

			return list;
		}

	}
}
