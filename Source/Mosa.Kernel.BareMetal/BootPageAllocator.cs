// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Runtime;

namespace Mosa.Kernel.BareMetal
{
	public class BootPageAllocator
	{
		private static Pointer BootReserveStartPage;
		private static uint BootReserveSize;
		private static uint UsedPages;

		internal static void Setup()
		{
			var start = Platform.GetBootReservedRegion();

			BootReserveStartPage = start.Address;
			BootReserveSize = (uint)start.Size / Page.Size;

			UsedPages = 0;
		}

		public static Pointer AllocatePage()
		{
			return AllocatePages(1);
		}

		public static Pointer AllocatePages(uint pages = 1)
		{
			// TODO: Acquire lock

			var result = BootReserveStartPage + (int)(UsedPages * Page.Size);

			UsedPages += pages;

			// TODO: Release lock

			return result;
		}
	}
}
