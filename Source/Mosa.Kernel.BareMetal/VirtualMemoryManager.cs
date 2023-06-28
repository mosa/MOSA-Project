// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Runtime;

namespace Mosa.Kernel.BareMetal;

public static class VirtualMemoryManager
{
	#region Private Members

	// List of page pools (representing physical pages)
	// Page pool consist of bitmap + # of entries + # of free entries

	#endregion Private Members

	#region Public API

	public static void Setup()
	{
		// TODO
		// Allocates the virtual memory map
		// Identity Map first x MB of memory
	}

	public static Pointer Reserve()
	{
		// TODO - Reserve a single page of virtual memory
		return Pointer.Zero;
	}

	public static Pointer Reserve(uint count)
	{
		// TODO -  Reserve pages of virtual memory
		return Pointer.Zero;
	}

	public static Pointer Release(Pointer virtualPage, uint count)
	{
		// TODO - Release pages of virtual memory
		return Pointer.Zero;
	}

	public static void Allocate(Pointer virtualPage)
	{
		// TODO - Allocate a physical page to a virtual memory space
		return;
	}

	public static void Map(Pointer virtualPage, Pointer physicalPage)
	{
		// TODO - Maps a virtual memory address to a physical page
		return;
	}

	public static Pointer GetPhysicalAddress(Pointer virtualPage)
	{
		// TODO - Returns the physical address of a virtual address
		return Pointer.Zero;
	}

	#endregion Public API
}
