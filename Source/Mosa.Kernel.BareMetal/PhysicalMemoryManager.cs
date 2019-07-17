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

		private static uint TotalPages = 0;

		private static uint MinimumAvailablePage = 0;
		private static uint MaximumAvailablePage = 0;
		private static uint MinimumReservedPage = 0;
		private static uint MaximumReservedPage = 0;

		private static uint SearchNextStartPage = 0;

		public static void Setup()
		{
			BitMapIndexTable = new BitMapIndexTable(BootPageAllocator.AllocatePage());

			var maximumMemoryAddress = BootMemoryMap.GetMaximumAddress();

			TotalPages = (uint)(maximumMemoryAddress.ToInt64() / Page.Size);
			var bitMapCount = TotalPages / (Page.Size * 8);

			for (uint i = 0; i < bitMapCount; i++)
			{
				var bitmap = BootPageAllocator.AllocatePage(1);
				Page.ClearPage(bitmap);

				BitMapIndexTable.AddBitMapEntry(i, bitmap);
			}

			var entries = BootMemoryMap.GetBootMemoryMapEntryCount();

			int pass = 0;

			// pass 0 - mark available pages
			// pass 1 - unmark reserved pages
			while (pass <= 1)
			{
				uint min = 0;
				uint max = 0;

				for (uint i = 0; i < entries; i++)
				{
					var entry = BootMemoryMap.GetBootMemoryMapEntry(i);

					if (pass == 0 && !entry.IsAvailable)
						continue;

					if (pass == 1 && entry.IsAvailable)
						continue;

					var start = Alignment.AlignUp(entry.StartAddress.ToInt64(), Page.Size);
					var end = Alignment.AlignDown(entry.EndAddress.ToInt64() + 1, Page.Size);

					var pages = (uint)(end - start) / Page.Size;

					if (pages <= 0)
						continue;

					var startPage = (uint)(start / Page.Shift);

					min = Math.Min(min, startPage);
					max = Math.Max(max, startPage + pages);

					SetPageBitMapEntry(startPage, pages, entry.IsAvailable);
				}

				if (pass == 0)
				{
					MinimumAvailablePage = min;
					MaximumAvailablePage = max;
				}
				else if (pass == 1)
				{
					MinimumReservedPage = min;
					MaximumReservedPage = max;
				}

				pass++;
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

		public static void ReleasePages(IntPtr page, uint count)
		{
			SetPageBitMapEntry((uint)page.ToInt64() / Page.Size, count, true);
		}

		public static IntPtr ReservePages(uint count, uint alignment)
		{
			if (count == 0)
				return IntPtr.Zero;

			if (alignment == 0)
				alignment = 1;

			uint start = SearchNextStartPage;
			uint at = start;

			while (true)
			{
				uint increment;

				if (at % alignment != 0)
				{
					// not aligned
					// todo - skip to next aligned address
					increment = 1;
				}
				else if (CheckFreePage(at, count, out uint breakPage))
				{
					// found space!

					SetPageBitMapEntry(at, count, true);

					SearchNextStartPage = at + 1;

					return new IntPtr(at * Page.Size);
				}
				else
				{
					increment = 1;
				}

				var nextAt = at + increment;

				if (nextAt > TotalPages || nextAt > MaximumAvailablePage)
				{
					// warp around to the start of the bitmap
					nextAt = MinimumAvailablePage;
				}

				if (nextAt > start && at < start)
				{
					// just loop around in the search
					// quit, there are no free pages
					return IntPtr.Zero;
				}

				at = nextAt;
			}
		}

		private static bool CheckFreePage(uint start, uint count, out uint breakPage)
		{
			breakPage = 0;

			if (start <= MinimumAvailablePage)
				return false;

			if (start + count > MaximumAvailablePage)
				return false;

			var indexShift = (IntPtr.Size == 4) ? 10 : 9;
			var maskOffIndex = (uint)((1 << (indexShift + 1)) - 1);

			var at = start;

			while (count > 0)
			{
				var index = (int)(at >> indexShift);

				var bitmap = BitMapIndexTable.GetBitMapEntry((uint)index);

				if (at % 64 == 0 && count >= 64)
				{
					// 64 bit check
					var offset = (uint)((index & maskOffIndex) >> 6);

					var value = bitmap.Load64(offset);

					if (value != 0)
					{
						// todo - calculate last searched
						return false;
					}

					at += 64;
					count -= 64;
				}
				else if (at % 32 == 0 && count >= 32)
				{
					// 32 bit check
					var offset = (uint)((index & maskOffIndex) >> 5);

					var value = bitmap.Load32(offset);

					if (value != 0)
					{
						// todo - calculate last searched
						return false;
					}

					at += 32;
					count -= 32;
				}
				else if (at % 8 == 0 && count >= 8)
				{
					// 8 bit check
					var offset = (uint)((index & maskOffIndex) >> 5);

					var value = bitmap.Load8(offset);

					if (value != 0)
					{
						// todo - calculate last searched
						return false;
					}
					at += 8;
					count -= 8;
				}
				else
				{
					// one bit check
					var offset = (uint)((index & maskOffIndex) >> 3);
					var value = bitmap.Load8(offset);

					var bit = (byte)(1 << index & 0b111) & value;

					if (bit != 0)
					{
						// todo - calculate last searched
						return false;
					}

					at += 1;
					count -= 1;
				}
			}

			return true;
		}
	}
}
