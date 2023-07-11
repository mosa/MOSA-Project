// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;
using Mosa.Kernel.BareMetal.BootMemory;
using Mosa.Runtime;

namespace Mosa.Kernel.BareMetal;

public static class PageFrameAllocator
{
	#region Public Members

	public static uint TotalPages { get; private set; }

	#endregion Public Members

	#region Private Members

	private static BitMapIndexTable BitMapIndexTable;

	private static Pointer AvailableMemory;

	private static uint MinimumAvailablePage;
	private static uint MaximumAvailablePage;
	private static uint MinimumReservedPage;
	private static uint MaximumReservedPage;

	private static uint SearchNextStartPage;

	#endregion Private Members

	#region Public API

	public static void Setup()
	{
		Debug.WriteLine("PageFrameAllocator:Setup()");

		var bitMapIndexPage = BootPageAllocator.AllocatePage();
		BitMapIndexTable = new BitMapIndexTable(bitMapIndexPage);

		AvailableMemory = BootMemoryMap.GetAvailableMemory();

		TotalPages = (uint)(AvailableMemory.ToUInt64() / Page.Size);
		var bitMapCount = TotalPages / (Page.Size * 8);

		for (var i = 0u; i < bitMapCount; i++)
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
		for (var i = 0u; i < entries; i++)
		{
			var entry = BootMemoryMap.GetBootMemoryMapEntry(i);

			if (!entry.IsAvailable)
				continue;

			var start = Alignment.AlignUp(entry.StartAddress.ToInt64(), Page.Size);
			var end = Alignment.AlignDown(entry.EndAddress.ToInt64(), Page.Size);

			var pages = (uint)(end - start) / Page.Size;

			if (pages <= 0)
				continue;

			var startPage = (uint)(start / Page.Size);
			var endPage = startPage + pages - 1;

			MinimumAvailablePage = Math.Min(MinimumAvailablePage, startPage);
			MaximumAvailablePage = Math.Max(MaximumAvailablePage, endPage);

			Debug.WriteLine(" > available: ", startPage, " - ", endPage);

			SetPageBitMapEntry(startPage, pages, true);
		}

		// pass 1 - unmark reserved pages
		for (var i = 0u; i < entries; i++)
		{
			var entry = BootMemoryMap.GetBootMemoryMapEntry(i);

			if (entry.IsAvailable)
				continue;

			var start = Alignment.AlignDown(entry.StartAddress.ToInt64(), Page.Size);
			var end = Alignment.AlignUp(entry.EndAddress.ToInt64(), Page.Size);

			//Debug.WriteLine(" > reserve");
			//Debug.WriteLineHex(" > start:         ", entry.StartAddress.ToInt64());
			//Debug.WriteLineHex(" > end:           ", entry.EndAddress.ToInt64());
			//Debug.WriteLine(" > len:           ", entry.Size);

			//Debug.WriteLineHex(" > start-aligned: ", start);
			//Debug.WriteLineHex(" > end-aligned:   ", end);

			var pages = (uint)(end - start) / Page.Size;

			if (pages <= 0)
				continue;

			var startPage = (uint)(start / Page.Size);
			var endPage = startPage + pages - 1;

			//Debug.WriteLine(" > pages: ", pages);
			//Debug.WriteLine(" > startPage: ", startPage);
			//Debug.WriteLine(" > endPage: ", endPage);

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

			//Debug.WriteLine(" > pages2: ", pages);

			Debug.WriteLine(" > reserved: ", startPage, " - ", endPage);

			SetPageBitMapEntry(startPage, pages, false);
		}

		// Reserve the first page
		SetPageBitMapEntry(1, 1, false);

		// TODO - reserve kernel code + memory

		SearchNextStartPage = MinimumAvailablePage;

		//Debug.Kill();
	}

	public static void Release(Pointer page, uint count)
	{
		SetPageBitMapEntry((uint)page.ToInt64() / Page.Size, count, true);
	}

	public static Pointer Allocate()
	{
		return Allocate(1, 1);
	}

	public static Pointer Allocate(uint count, uint alignment = 1)
	{
		Debug.WriteLine("PageFrameAllocator::Allocate()");
		Debug.WriteLine(" > Reserve Pages: ", count);

		if (count == 0)
			return Pointer.Zero;

		if (alignment == 0)
			alignment = 1;

		// TODO: Acquire lock

		var start = SearchNextStartPage;
		var at = start;
		var wrap = false;

		while (true)
		{
			uint restartAt;

			//Debug.WriteLine(" > @ ", at);

			if (at % alignment != 0)
			{
				// not aligned
				// todo - skip to next aligned address rather then just increment
				restartAt = at + 1;
			}
			else if (CheckFreePage32(at, count, out restartAt))
			{
				// found space!
				SetPageBitMapEntry(at, count, true);

				SearchNextStartPage = restartAt;

				//Debug.WriteLine(" > return: ", at);

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

				if (wrap)
					Debug.Kill();

				wrap = true;
			}

			if (at < start && restartAt > start)
			{
				// looped around in the search
				// quit, as there are no free pages

				//Debug.WriteLine(" > return: Zero");

				return Pointer.Zero;
			}

			at = restartAt;
		}
	}

	public static void ReservePage(uint page)
	{
		SetPageBitMapEntry(page, 1, false);
	}

	public static void ReservePages(uint pages, uint count)
	{
		SetPageBitMapEntry(pages, count, false);
	}

	public static uint ConvertToPageNumber(Pointer page)
	{
		return (uint)page.ToUInt64() % Page.Size;
	}

	#endregion Public API

	#region Private API

	private static void SetPageBitMapEntry(uint start, uint count, bool set)
	{
		var at = start;

		// TODO: Acquire lock

		while (count > 0)
		{
			var index = at >> 15;            // upper 15+ bits
			var bitoffset = at & 0x7FFF;     // lower 15 bits
			var offset = bitoffset >> 3;     // byte offset

			var bitmap = BitMapIndexTable.GetBitMapEntry(index);

			if (at % 64 == 0 && count >= 64 && bitoffset < (4096 - 64))
			{
				// 64 bit update
				bitmap.Store64(offset, set ? ulong.MaxValue : 0);

				at += 64;
				count -= 64;
			}
			else if (at % 32 == 0 && count >= 32 && bitoffset < (4096 - 32))
			{
				// 32 bit update
				bitmap.Store32(offset, set ? uint.MaxValue : 0);

				at += 32;
				count -= 32;
			}
			else if (at % 8 == 0 && count >= 8 && bitoffset < (4096 - 8))
			{
				// 8 bit update
				bitmap.Store8(offset, set ? byte.MaxValue : (byte)0);

				at += 8;
				count -= 8;
			}
			else
			{
				// one bit at a time
				var value = bitmap.Load8(offset);

				var shift = at & 0b111;
				var bit = (byte)(1 << ((int)shift));

				value = (byte)(set ? (value | bit) : (value & ~bit));

				bitmap.Store8(offset, value);

				at += 1;
				count -= 1;
			}
		}
	}

	private static bool CheckFreePage32(uint at, uint count, out uint nextAt)
	{
		nextAt = at;

		//Debug.WriteLine("CheckFreePage32:PhysicalPageAllocator()");
		//Debug.WriteLine(" > count = ", count);

		while (count > 0)
		{
			var index = at >> 15;              // upper 15+ bits -> index to bitmap
			var bitoffset = at & 0x7FFF;       // lower 15 bits -> bit in bitmap
			var offset = (bitoffset >> 5) * 4; // offset to 32bit value

			var startbit = at & 0b11111;       // 0x1F = 5 bits on

			var bitlen = Math.Min(32 - startbit, count);    // NOTE: 32bit specific
			var mask = bitlen == 32 ? uint.MaxValue : (~(uint.MaxValue << (int)bitlen)) << (int)startbit; // NOTE: 32bit specific

			var bitmap = BitMapIndexTable.GetBitMapEntry(index); // NOTE: 32bit specific
			var value = bitmap.Load32(offset);

			var maskvalue = (~value) & mask;

			Debug.Assert(bitlen != 0, "PhysicalPageAllocator::CheckFreePage32() -> bitlen != 0");

			if (maskvalue == 0)
			{
				nextAt = at + bitlen;
				count -= bitlen;
				at += bitlen;
			}
			else
			{
				// Future optimization: return nextAt page of the first available bit after the first unavailble bit, and if not, then start at next 32-bit aligned page number
				nextAt = at + 1;

				return false;
			}
		}

		return true;
	}

	#endregion Private API
}
