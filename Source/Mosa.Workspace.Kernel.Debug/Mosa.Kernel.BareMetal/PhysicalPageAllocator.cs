// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;
using Mosa.Kernel.BareMetal.BootMemory;
using Mosa.Runtime;

namespace Mosa.Kernel.BareMetal;

public static class PhysicalPageAllocator
{
	private static BitMapIndexTable BitMapIndexTable;

	private static Pointer AvailableMemory;

	private static uint TotalPages = 0;

	private static uint MinimumAvailablePage = 0;
	private static uint MaximumAvailablePage = 0;
	private static uint MinimumReservedPage = 0;
	private static uint MaximumReservedPage = 0;

	private static uint SearchNextStartPage = 0;

	public static void Setup()
	{
		AvailableMemory = BootMemoryMap.GetAvailableMemory();
		TotalPages = (uint)(AvailableMemory.ToInt64() / Page.Size);

		var bitMapCount = TotalPages / (Page.Size * 8);

		var table = BootPageAllocator.AllocatePages(bitMapCount);

		BitMapIndexTable = new BitMapIndexTable(table);

		for (uint i = 0; i < bitMapCount; i++)
		{
			var bitmap = BootPageAllocator.AllocatePage();
			Page.ClearPage(bitmap);

			BitMapIndexTable.AddBitMapEntry(i, bitmap);
		}

		MinimumAvailablePage = TotalPages;
		MaximumAvailablePage = 0;
		MinimumReservedPage = TotalPages;
		MaximumReservedPage = 0;

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

			SetPageBitMapEntry32(startPage, pages, true);
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
				MinimumAvailablePage = endPage + 1;

			if (MaximumAvailablePage >= startPage && MaximumAvailablePage <= endPage)
				MaximumAvailablePage = startPage - 1;

			if (startPage >= TotalPages)
				continue;

			if (endPage > TotalPages)
				pages = TotalPages - startPage;

			SetPageBitMapEntry32(startPage, pages, false);
		}

		SearchNextStartPage = MinimumAvailablePage;
	}

	public static void ReleasePages(Pointer page, uint count)
	{
		SetPageBitMapEntry32((uint)page.ToInt64() / Page.Size, count, true);
	}

	public static Pointer ReservePage()
	{
		return ReservePages(1, 1);
	}

	public static Pointer ReservePages(uint count, uint alignment = 1)
	{
		if (count == 0)
			return Pointer.Zero;

		if (alignment == 0)
			alignment = 1;

		// TODO: Acquire lock

		uint start = SearchNextStartPage;
		uint at = start;

		while (true)
		{
			uint restartAt;

			if (at % alignment != 0)
			{
				// not aligned
				// todo - skip to next aligned address
				restartAt = at + 1;
			}
			else if (CheckFreePage32(at, count, out restartAt))
			{
				// found space!

				SetPageBitMapEntry32(at, count, true);

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

	private static void SetPageBitMapEntry32(uint start2, uint count, bool set)
	{
		uint at = start2;

		while (count > 0)
		{
			uint index = at >> 17;
			uint slot = index * 4;

			var bitmap = BitMapIndexTable.GetBitMapEntry(slot);

			byte start = (byte)(at & 0b11111);
			uint diff = start + count - 1;
			byte end = (byte)(diff <= 31 ? diff : 31);
			byte size = (byte)(end - start + 1);

			uint mask = BitSet32(start, size);

			uint offset32 = (at >> 10) & 0b11111;

			var value = bitmap.Load32(offset32);

			uint newvalue = set ? (value | mask) : (value & ~mask);

			bitmap.Store32(offset32, newvalue);

			count -= size;
			at += size;
		}
	}

	private static bool CheckFreePage32(uint at, uint count, out uint nextAt)
	{
		nextAt = at;

		while (count > 0)
		{
			uint index = at >> 17;
			uint slot = index * 4;

			var bitmap = BitMapIndexTable.GetBitMapEntry(slot);

			byte start = (byte)(at & 0b11111);
			uint diff = start + count - 1;
			byte end = (byte)(diff <= 31 ? diff : 31);
			byte size = (byte)(end - start + 1);

			uint mask = BitSet32(start, size);

			uint offset32 = (at >> 10) & 0b11111;

			var value = bitmap.Load32(offset32);

			if ((value & mask) == 0)
			{
				nextAt = at + size;
				count -= size;
				at += size;
			}
			else
			{
				nextAt = at + 1; // + GetLowestSetBit(value)
				return false;
			}
		}

		return true;
	}

	private static uint BitSet32(byte start, byte size)
	{
		if (size == 32)
			return uint.MaxValue;

		return ~(uint.MaxValue << size) << start;
	}

	private static byte GetLowestSetBit(uint value)
	{
		byte index = 0;

		while ((value & 1) == 0)
		{
			value >>= 1;
			index++;
		}

		return index;
	}
}
