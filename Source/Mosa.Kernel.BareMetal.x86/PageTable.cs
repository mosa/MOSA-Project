// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Runtime;
using Mosa.Runtime.x86;

namespace Mosa.Kernel.BareMetal.x86
{
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
			Console.WriteLine("Mosa.Kernel.BareMetal.x86.PageTable.Setup:Enter");
			GDTTable = new GDTTable(PhysicalPageAllocator.ReservePage());

			PageDirectory = PhysicalPageAllocator.ReservePage();

			PageTables = PhysicalPageAllocator.ReservePages(1024);

			Console.WriteLine("Mosa.Kernel.BareMetal.x86.PageTable.Setup:Exit");
		}

		public static void Initialize()
		{
			Console.WriteLine("Mosa.Kernel.BareMetal.x86.PageTable.Initialize:Enter");

			GDTTable.Setup();

			Console.WriteLine("Mosa.Kernel.BareMetal.x86.PageTable.Initialize:1");

			// Setup Page Directory
			for (uint index = 0; index < 1024; index++)
			{
				PageDirectory.Store32(index << 2, (uint)(PageTables.ToInt32() + (index * 4096) | 0x04 | 0x02 | 0x01));
			}

			Console.WriteLine("Mosa.Kernel.BareMetal.x86.PageTable.Initialize:2");

			// Clear the Page Tables
			for (uint index = 0; index < 1024; index++)
			{
				Console.WriteValue(index);

				Page.ClearPage(PageTables + (index * Page.Size));

				Console.Write(' ');
			}

			Console.WriteLine("Mosa.Kernel.BareMetal.x86.PageTable.Initialize:Exit");
			while (true) { }
		}

		public static void Enable()
		{
			GDTTable.Enable();

			// Set CR3 register on processor - sets page directory
			Native.SetCR3((uint)PageDirectory.ToInt32());

			// Set CR0 register on processor - turns on virtual memory
			Native.SetCR0(Native.GetCR0() | 0x80000000);
		}

		public static void MapVirtualAddressToPhysical(uint virtualAddress, uint physicalAddress, bool present = true)
		{
			//FUTURE: traverse page directory from CR3 --- do not assume page table is linearly allocated

			PageTables.Store32((virtualAddress & 0xFFFFF000u) >> 10, physicalAddress & 0xFFFFF000u | 0x04u | 0x02u | (present ? 0x1u : 0x0u));
		}

		public static Pointer GetPhysicalAddressFromVirtual(Pointer virtualAddress)
		{
			//FUTURE: traverse page directory from CR3 --- do not assume page table is linearly allocated

			var address = (uint)virtualAddress.ToInt32();
			var offset = ((address & 0xFFFFF000u) >> 10) + (address & 0xFFFu);

			return PageTables.LoadPointer(offset);
		}
	}
}
