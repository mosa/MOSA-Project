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

	#region Constants

	private static class Constant
	{
		public const uint Present = 0x01;
		public const uint ReadWrite = 0x02;
		public const uint UserSupervisor = 0x04;
		public const uint WriteThrough = 0x08;
		public const uint CacheDisable = 1 << 4;
		public const uint Accessed = 1 << 5;
		public const uint Dirty = 1 << 6;
	}

	#endregion Constants

	public const uint PageTableCount = 1024;
	public const uint IdentityPages = 32768; // 128MB

	public static void Setup()
	{
		Debug.WriteLine("x86.PageTable:Setup()");

		GDTTable.Setup();
		PageDirectory = PageFrameAllocator.Allocate();
		PageTables = PageFrameAllocator.Allocate(PageTableCount);

		Debug.WriteLine(" > Page Directory @ ", new Hex(PageDirectory));
		Debug.WriteLine(" > Page Table @ ", new Hex(PageTables));

		Debug.WriteLine("x86.PageTable:Setup() [Exit]");
	}

	public static void Initialize()
	{
		Debug.WriteLine("x86.PageTable:Initialize()");

		// Setup Page Directory
		Debug.WriteLine(" * Setup Page Directory");
		for (var index = 0u; index < PageTableCount; index++)
		{
			PageDirectory.Store32(index << 2, (PageTables.ToUInt32() + index * Page.Size) | Constant.Present | Constant.ReadWrite | Constant.UserSupervisor);
		}

		Debug.WriteLine(" * Clearing Page Table Entries");
		PageTables.Clear(PageTableCount * Page.Size);

		// Setup Identity Pages
		Debug.WriteLine(" * Setup Identity Pages");
		MapIdentityPages(PageTables, PageTableCount);
		MapIdentityPages(PageDirectory, 1);

		MapIdentityPages(Pointer.Zero, IdentityPages);

		Debug.WriteLine("x86.PageTable:Initialize() [Exit]");
	}

	private static void MapIdentityPages(Pointer start, uint pages)
	{
		Debug.WriteLine("x86.PageTable:MapIdentityPages()");
		Debug.WriteLine(" > Start: ", new Hex(start));

		var end = start + (pages * Page.Size);

		Debug.WriteLine(" > End: ", new Hex(end));

		for (var page = start; page < end; page += Page.Size)
		{
			MapVirtualAddressToPhysical(page, page, true);
		}

		Debug.WriteLine("x86.PageTable:MapIdentityPages() [Exit]");
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
		PageTables.Store32((virtualAddress.ToUInt32() & 0xFFFFF000u) >> 10, (physicalAddress.ToUInt32() & 0xFFFFF000u) | (present ? Constant.Present : 0x0u) | Constant.ReadWrite | Constant.UserSupervisor);
	}

	public static Pointer GetPhysicalAddressFromVirtual(Pointer virtualAddress)
	{
		var address = virtualAddress.ToUInt32();
		var offset = ((address & 0xFFFFF000u) >> 10) + (address & 0xFFFu);

		return PageTables.LoadPointer(offset);
	}
}
