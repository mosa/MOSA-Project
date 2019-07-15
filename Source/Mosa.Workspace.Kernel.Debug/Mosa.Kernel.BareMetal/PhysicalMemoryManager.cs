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
				var end = Alignment.AlignDown(entry.EndAddress.ToInt64() + 1, Page.Size);

				var pages = (end - start) / Page.Size;

				if (pages <= 0)
					continue;

				var startPage = start / Page.Shift;

				SetPageBitMapEntry((uint)startPage, (uint)pages, true);
			}
		}

		private static void SetPageBitMapEntry(uint start, uint count, bool set)
		{
			var indexShift = (IntPtr.Size == 4) ? 10 : 9;
			var maskOffIndex = (uint)((1 << (indexShift + 1)) - 1);

			var at = start;

			while (count > 0)
			{
				var index = (int)(at >> indexShift);

				var bitmap = BitMapIndexTable.GetBitMapEntry((uint)index);

				if (at % 64 == 0 && count >= 64)
				{
					// 64 bit update
					var offset = (uint)((index & maskOffIndex) >> 6);

					bitmap.Store64(offset, set ? ulong.MaxValue : 0);

					at += 64;
					count -= 64;
				}
				else if (at % 32 == 0 && count >= 32)
				{
					// 32 bit update
					var offset = (uint)((index & maskOffIndex) >> 5);

					bitmap.Store32(offset, set ? uint.MaxValue : 0);

					at += 32;
					count -= 32;
				}
				else if (at % 8 == 0 && count >= 8)
				{
					// 8 bit update
					var offset = (uint)((index & maskOffIndex) >> 5);

					bitmap.Store8(offset, set ? byte.MaxValue : (byte)0);

					at += 8;
					count -= 8;
				}
				else
				{
					// one bit update
					var offset = (uint)((index & maskOffIndex) >> 3);
					var value = bitmap.Load8(offset);

					var bit = (byte)(1 << index & 0b111);
					value = (byte)(set ? value | bit : value & bit);

					bitmap.Store8(offset, value);

					at += 1;
					count -= 1;
				}
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
