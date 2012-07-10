/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com> 
 */


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

		private static uint PageSize { get { return 4096; } }
		private const uint ReserveMemory = 1024 * 1024 * 32; 
		private static uint _pages;
		private static bool GetPageStatus(uint page)  // true = available
		{
			return true;
		}
		private static void SetPageStatus(uint page, bool free)
		{
		}

		public static uint Test1(uint size)
		{
			uint first = 0xFFFFFFFF; // Marker
			uint pages = ((size - 1) / PageSize) + 1;

			for (uint at = 0; at < _pages; at++)
			{
				if (GetPageStatus(at))
				{
					if (first == 0xFFFFFFFF)
						first = at;

					if (at - first == pages)
					{
						for (uint index = 0; index < pages; index++)
							SetPageStatus(first + index, false);

						return ((first * PageSize) + ReserveMemory);
					}
				}
				else
					first = 0xFFFFFFFF;
			}

			return first;
		}

		public static int R4ToI4(float value)
		{
			return (int)value;
		}

		public static int R8ToI4(double value)
		{
			return (int)value;
		}

	}
}