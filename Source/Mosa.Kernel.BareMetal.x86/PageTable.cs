// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Runtime;
using Mosa.Runtime.x86;

namespace Mosa.Kernel.BareMetal.x86;

/// <summary>
/// Page Table
/// </summary>
internal static class PageTable
{
	public static Pointer PageDirectory;
	public static Pointer PageTables;
	public static GDT GDTTable;

	public static void Setup()
	{
		Debug.WriteLine("x86.PageTable:Setup()");

		GDTTable.Setup();
		PageDirectory = PageFrameAllocator.Allocate(1024);
		PageTables = PageFrameAllocator.Allocate(1024);

		Debug.WriteLine("x86.PageTable:Setup() [Exit]");
	}

	public static void Initialize()
	{
		Debug.WriteLine("x86.PageTable:Initialize()");

		// Setup Page Directory
		Debug.WriteLine(" > Setup Page Directory");
		for (uint index = 0; index < 1024; index++)
		{
			PageDirectory.Store32(index << 2, (PageTables.ToUInt32() + index * Page.Size) | 0x04 | 0x02 | 0x01);
		}

		// Clear the Page Tables
		Debug.WriteLine(" > Clear the Page Tables");
		for (uint index = 0; index < PageFrameAllocator.TotalPages; index++)
		{
			PageTables.Store32(index << 2, (index * Page.Size) | 0x04 | 0x02 | 0x01);
		}

		// Setup Identity Pages
		Debug.WriteLine(" > Setup Identity Pages");

		// Map the first 128MB of memory
		var endPage = new Pointer(128 * 1024 * 1024);

		for (var page = Pointer.Zero; page < endPage; page += Page.Size)
		{
			MapVirtualAddressToPhysical(page, page, true);
		}

		Debug.WriteLine("x86.PageTable:Initialize() [Exit]");
	}

	public static void Enable()
	{
		Debug.WriteLine("x86.PageTable:Enable()");

		// Set CR3 register on processor - sets page directory
		Native.SetCR3(PageDirectory.ToUInt32());

		// Set CR0 register on processor - turns on virtual memory
		Native.SetCR0(Native.GetCR0() | 0x80000000);

		Debug.WriteLine("x86.PageTable:Enable() [Exit]");
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
