﻿// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Runtime;

namespace Mosa.Kernel.BareMetal;

/// <summary>
/// Page Table
/// </summary>
public static class PageTable
{
	public static void Setup()
	{
		Console.WriteLine("Mosa.Kernel.BareMetal.PageTable.Setup:Enter");
		Platform.PageTableSetup();

		Console.WriteLine("Mosa.Kernel.BareMetal.PageTable.Setup:1");
		Platform.PageTableInitialize();

		Console.WriteLine("Mosa.Kernel.BareMetal.PageTable.Setup:2");

		while (true) { }

		// Unmap the first page for null pointer exceptions
		MapVirtualAddressToPhysical(0x0, 0x0, false);
		Console.Write("d");

		Platform.PageTableEnable();
		Console.Write("e");
	}

	/// <summary>
	/// Maps the virtual address to physical.
	/// </summary>
	/// <param name="virtualAddress">The virtual address.</param>
	/// <param name="physicalAddress">The physical address.</param>
	public static void MapVirtualAddressToPhysical(uint virtualAddress, uint physicalAddress, bool present = true)
	{
	}

	/// <summary>
	/// Gets the physical memory.
	/// </summary>
	/// <param name="virtualAddress">The virtual address.</param>
	/// <returns></returns>
	public static Pointer GetPhysicalAddressFromVirtual(Pointer virtualAddress)
	{
		return Pointer.Zero;
	}
}
