/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System;

namespace Mosa.Test.Collection
{
	public static class SpecificTests
	{
		public static uint Test2(uint size)
		{
			uint first = 0xFFFFFFFF; // Marker

			for (uint at = 0; at < 10; at++)
			{
				if (first == 1)
				{
					if (first == 0xFFFFFFFF)
						first = at;
				}
				else
					first = 0xFFFFFFFF;
			}

			return first;
		}

		//public static bool R8IsPositiveInfinityTrue()
		//{
		//	return Double.IsPositiveInfinity(Double.PositiveInfinity);
		//}

		//public static bool R8IsNegativeInfinityFalse()
		//{
		//	return Double.IsNegativeInfinity(Double.PositiveInfinity);
		//}

		//public static bool R8IsNegativeInfinityTrue()
		//{
		//	return Double.IsNegativeInfinity(Double.NegativeInfinity);
		//}

		//public static bool R8IsPositiveInfinityFalse()
		//{
		//	return Double.IsPositiveInfinity(Double.NegativeInfinity);
		//}

	}
}