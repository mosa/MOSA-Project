// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;

namespace Mosa.Kernel.BareMetal
{
	internal class BootPageAllocator
	{
		private static IntPtr BootReserveStartPage;
		private static uint BootReserveSize;
		private static uint UsedPages;

		public static void Setup()
		{
			var start = Platform.GetBootReservedRegion();

			BootReserveStartPage = start.Address;
			BootReserveSize = (uint)start.Size;

			UsedPages = 0;
		}

		public static IntPtr AllocatePage(uint pages = 1)
		{
			var result = BootReserveStartPage + (int)(UsedPages * Page.Size);

			UsedPages += pages;

			return result;
		}
	}
}
