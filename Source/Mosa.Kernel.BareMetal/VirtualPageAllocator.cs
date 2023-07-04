﻿// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Runtime;

namespace Mosa.Kernel.BareMetal;

public static class VirtualPageAllocator
{
	#region Private Members

	// FUTURE:
	// List of page pools (representing physical pages)
	// Page pool consist of bitmap + # of entries + # of free entries

	private static ulong NextAvailablePage;

	#endregion Private Members

	#region Public API

	public static void Setup()
	{
		// TODO
		// Allocates the virtual memory map
		// Identity Map first x MB of memory

		NextAvailablePage = 0x40000000 / Page.Size;    // 1GB
	}

	public static Pointer ReservePage()
	{
		return ReservePages(1);
	}

	public static Pointer ReservePages(uint count = 1)
	{
		var available = new Pointer(NextAvailablePage * Page.Size);
		NextAvailablePage += count;

		return available;
	}

	public static void Release(Pointer virtualPage, uint count)
	{
		// TODO - Release pages of virtual memory
	}

	public static Pointer Allocate(Pointer virtualPage)
	{
		var physicalPage = PageFrameAllocator.Allocate();
		Map(virtualPage, physicalPage);

		return physicalPage;
	}

	public static void Map(Pointer virtualPage, Pointer physicalPage)
	{
		PageTable.MapVirtualAddressToPhysical(virtualPage, physicalPage, true);
	}

	public static Pointer GetPhysicalAddress(Pointer virtualPage)
	{
		return PageTable.GetPhysicalAddressFromVirtual(virtualPage);
	}

	#endregion Public API
}
