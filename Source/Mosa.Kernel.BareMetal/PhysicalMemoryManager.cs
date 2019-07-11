// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Kernel.BareMetal.BootMemory;
using Mosa.Kernel.BareMetal.Extension;
using Mosa.Runtime.Extension;
using System;

namespace Mosa.Kernel.BareMetal
{
	public static class PhysicalMemoryManager
	{
		private static BitMapIndexTable BitMapIndexTable;

		public static void Setup()
		{
			BitMapIndexTable = new BitMapIndexTable(BootPageAllocator.AllocatePage());

			var maximumMemoryAddress = BootMemoryMap.GetMaximumAddress();
			var maximumPage = maximumMemoryAddress.ToInt64() / Page.Size;
			var bitMapCount = maximumPage / (Page.Size * 8);

			for (uint i = 0; i < bitMapCount; i++)
			{
				var bitmap = BootPageAllocator.AllocatePage(1);
				Page.ClearPage(bitmap);

				BitMapIndexTable.AddBitMapEntry(i, bitmap);
			}

			var entries = BootMemoryMap.GetBootMemoryMapEntryCount();

			for (uint i = 0; i < entries; i++)
			{
				var entry = BootMemoryMap.GetBootMemoryMapEntry(i);

				if (entry.IsAvailable)
					continue;

				var start = Alignment.AlignUp(entry.StartAddress.ToInt64(), Page.Size);
				var pages = ((ulong)entry.EndAddress.ToInt64() + 1 - start) / Page.Size;

				if (pages <= 0)
					continue;

				var startPage = start / Page.Shift;

				MarkPagesAvailable(startPage, pages);
			}
		}

		private static void MarkPagesAvailable(ulong start, ulong count)
		{
			// slow trivial implementation

			var end = start + count;

			for (var page = start; page < end; page++)
			{
				// find table index
				var index = page >> ((int)Page.Shift + 4);

				var bitmap = BitMapIndexTable.GetBitMapEntry((uint)index);

				var offset = page >> ((int)Page.Shift) & Page.Shift;

				byte value = bitmap.Load8((uint)offset);

				var bit = index & 0b1111;

				// TODO
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
