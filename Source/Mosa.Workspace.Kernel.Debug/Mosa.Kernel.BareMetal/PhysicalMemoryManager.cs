// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Kernel.BareMetal.BootMemory;
using Mosa.Kernel.BareMetal.Extension;
using System;

namespace Mosa.Kernel.BareMetal
{
	public static class PhysicalMemoryManager
	{
		private static IntPtr BitMapIndexTable;

		public static void Start()
		{
			BitMapIndexTable = BootPageAllocator.AllocatePage();
			Page.ClearPage(BitMapIndexTable);

			var maximumMemoryAddress = BootMemoryMap.GetMaximumAddress();
			var maximumPage = maximumMemoryAddress.ToInt64() / Page.Size;
			var bitMapCount = maximumPage / (Page.Size * 8);

			for (uint i = 0; i < bitMapCount; i++)
			{
				var bitmap = BootPageAllocator.AllocatePage(1);
				Page.ClearPage(bitmap);

				BitMapIndexTable.StorePointer((uint)(IntPtr.Size * i), bitmap);
			}
		}

		public static IntPtr ReservePages(int count, int alignment)
		{
			//TODO
			return IntPtr.Zero;
		}

		public static IntPtr ReserveAnyPage()
		{
			//TODO
			return IntPtr.Zero; // new AddressRange(0, 1);
		}

		public static void ReleasePages(IntPtr page, int count)
		{
			//TODO
		}
	}
}
