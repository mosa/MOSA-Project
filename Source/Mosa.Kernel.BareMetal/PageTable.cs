// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Runtime;

namespace Mosa.Kernel.BareMetal;

/// <summary>
/// Page Table
/// </summary>
public static class PageTable
{
	public static void Setup()
	{
		Debug.WriteLine("PageTable:Setup()");

		Platform.PageTable.Setup();
		Platform.PageTable.Initialize();

		//// Unmap the first page for null pointer exceptions
		//MapVirtualAddressToPhysical(Pointer.Zero, Pointer.Zero, false);

		Platform.PageTable.Enable();

		Debug.WriteLine("PageTable:Setup() [Exit]");
	}

	/// <summary>
	/// Maps the virtual address to physical.
	/// </summary>
	/// <param name="virtualAddress">The virtual address.</param>
	/// <param name="physicalAddress">The physical address.</param>
	public static void MapVirtualAddressToPhysical(Pointer virtualAddress, Pointer physicalAddress, bool present = true)
	{
		Platform.PageTable.MapVirtualAddressToPhysical(virtualAddress, physicalAddress, present);
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
