// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Runtime;
using Mosa.Runtime.x86;
using Mosa.Kernel.BareMetal;

namespace Mosa.Kernel.BareMetal.x86;

/// <summary>
/// Page Table
/// </summary>
internal static class PageTable
{
	public static Pointer PageDirectory;
	public static Pointer PageTables;
	public static GDTTable GDTTable;

	public static void Setup()
	{
		Debug.WriteLine(ConsoleColor.BrightMagenta, "PageTable:Setup()");

		GDTTable = new GDTTable(PhysicalPageAllocator.Allocate());
		Debug.WriteLine(ConsoleColor.BrightMagenta, " > GDTTable");

		PageDirectory = PhysicalPageAllocator.Allocate(1024);
		Debug.WriteLine(ConsoleColor.BrightMagenta, " > PageDirectory");

		PageTables = PhysicalPageAllocator.Allocate(1024);
		Debug.WriteLine(ConsoleColor.BrightMagenta, " > PageTables");
	}

	public static void Initialize()
	{
		GDTTable.Setup();

		// Setup Page Directory
		for (uint index = 0; index < 1024; index++)
		{
			PageDirectory.Store32(index << 2, (PageTables.ToUInt32() + index * Page.Size) | 0x04 | 0x02 | 0x01);
		}

		// Clear the Page Tables
		for (uint index = 0; index < PhysicalPageAllocator.TotalPages; index++)
		{
			PageTables.Store32(index << 2, (index * Page.Size) | 0x04 | 0x02 | 0x01);
		}
	}

	public static void Enable()
	{
		GDTTable.Enable();

		// Set CR3 register on processor - sets page directory
		Native.SetCR3(PageDirectory.ToUInt32());

		// Set CR0 register on processor - turns on virtual memory
		Native.SetCR0(Native.GetCR0() | 0x80000000);
	}

	public static void MapVirtualAddressToPhysical(Pointer virtualAddress, Pointer physicalAddress, bool present = true)
	{
		//FUTURE: traverse page directory from CR3 --- do not assume page table is linearly allocated

		PageTables.Store32((virtualAddress.ToUInt32() & 0xFFFFF000u) >> 10, physicalAddress.ToUInt32() & 0xFFFFF000u | 0x04u | 0x02u | (present ? 0x1u : 0x0u));
	}

	public static Pointer GetPhysicalAddressFromVirtual(Pointer virtualAddress)
	{
		//FUTURE: traverse page directory from CR3 --- do not assume page table is linearly allocated

		var address = virtualAddress.ToUInt32();
		var offset = ((address & 0xFFFFF000u) >> 10) + (address & 0xFFFu);

		return PageTables.LoadPointer(offset);
	}
}
