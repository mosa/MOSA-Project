// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Kernel.BareMetal.BootMemory;
using Mosa.Runtime;
using System;

namespace Mosa.Kernel.BareMetal
{
	public static class PhysicalPageAllocator
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
				var bitmap = BootPageAllocator.AllocatePage();
				Page.ClearPage(bitmap);

				BitMapIndexTable.AddBitMapEntry(i, bitmap);
			}

			var entries = BootMemoryMap.GetBootMemoryMapEntryCount();

			// pass 0 - mark available pages
			for (uint i = 0; i < entries; i++)
			{
				var entry = BootMemoryMap.GetBootMemoryMapEntry(i);

				if (!entry.IsAvailable)
					continue;

				var start = Alignment.AlignUp(entry.StartAddress.ToInt64(), Page.Size);
				var end = Alignment.AlignDown(entry.EndAddress.ToInt64() + 1, Page.Size);

				var pages = (uint)(end - start) / Page.Size;

				if (pages <= 0)
					continue;

				var startPage = (uint)(start / Page.Shift);
				var endPage = startPage + pages - 1;

				MinimumAvailablePage = Math.Min(MinimumAvailablePage, startPage);
				MaximumAvailablePage = Math.Max(MaximumAvailablePage, endPage);

				SetPageBitMapEntry(startPage, pages, entry.IsAvailable);
			}

			// pass 1 - unmark reserved pages
			for (uint i = 0; i < entries; i++)
			{
				var entry = BootMemoryMap.GetBootMemoryMapEntry(i);

				if (entry.IsAvailable)
					continue;

				var start = Alignment.AlignUp(entry.StartAddress.ToInt64(), Page.Size);
				var end = Alignment.AlignDown(entry.EndAddress.ToInt64() + 1, Page.Size);

				var pages = (uint)(end - start) / Page.Size;

				if (pages <= 0)
					continue;

				var startPage = (uint)(start / Page.Shift);
				var endPage = startPage + pages - 1;

				MinimumReservedPage = Math.Min(MinimumReservedPage, startPage);
				MaximumReservedPage = Math.Max(MaximumReservedPage, endPage);

				if (MinimumAvailablePage >= startPage && MinimumAvailablePage <= endPage)
				{
					MinimumAvailablePage = endPage + 1;
				}

				if (MaximumAvailablePage >= startPage && MaximumAvailablePage <= endPage)
				{
					MaximumAvailablePage = startPage - 1;
				}

				SetPageBitMapEntry(startPage, pages, entry.IsAvailable);
			}

			SearchNextStartPage = MinimumAvailablePage;
		}

		private static void SetPageBitMapEntry(uint start, uint count, bool set)
		{
			var indexShift = (Pointer.Size == 4) ? 10 : 9;
			var maskOffIndex = (uint)((1 << (indexShift + 1)) - 1);

			var at = start;

			// TODO: Acquire lock

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

		public static void ReleasePages(Pointer page, uint count)
		{
			SetPageBitMapEntry((uint)page.ToInt64() / Page.Size, count, true);
		}

		public static Pointer ReservePage()
		{
			return ReservePages(1, 1);
		}

		public static Pointer ReservePages(uint count, uint alignment = 1)
		{
			Console.Write("z");

			if (count == 0)
				return Pointer.Zero;

			if (alignment == 0)
				alignment = 1;

			// TODO: Acquire lock
			Console.Write("y");

			uint start = SearchNextStartPage;
			uint at = start;

			while (true)
			{
				Console.Write("x");
				uint restartAt;

				if (at % alignment != 0)
				{
					// not aligned
					// todo - skip to next aligned address
					restartAt = at + 1;
				}
				else if (CheckFreePage(at, count, out restartAt))
				{
					// found space!

					SetPageBitMapEntry(at, count, true);

					SearchNextStartPage = restartAt;

					return new Pointer(at * Page.Size);
				}
				else
				{
					at = restartAt;
				}

				if (restartAt > MaximumAvailablePage || restartAt > TotalPages)
				{
					// warp around to the start of the bitmap
					restartAt = MinimumAvailablePage;
				}

				if (at < start && restartAt > start)
				{
					// looped around in the search
					// quit, as there are no free pages
					return Pointer.Zero;
				}

				at = restartAt;
			}
		}

		private static bool CheckFreePage(uint start, uint count, out uint restartAt)
		{
			//if (start < MinimumAvailablePage)
			//{
			//	// should never happen
			//	restartAt = MinimumAvailablePage;
			//	return false;
			//}

			var end = start + count;

			if (end > MaximumAvailablePage || end > TotalPages)
			{
				restartAt = MinimumAvailablePage;
				return false;
			}

			var indexShift = (Pointer.Size == 4) ? 10 : 9;
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

					if (value != ulong.MaxValue)
					{
						restartAt = at + 64;
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

					if (value != uint.MaxValue)
					{
						restartAt = at + 32;
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

					if (value != byte.MaxValue)
					{
						restartAt = at + 8;
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
						restartAt = at + 1;
						return false;
					}

					at += 1;
					count -= 1;
				}
			}

			restartAt = start + count + 1;

			return true;
		}
	}
}
