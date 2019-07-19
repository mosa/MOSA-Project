// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Kernel.BareMetal.Extension;
using Mosa.Runtime.Extension;
using Mosa.Runtime.x86;
using System;

namespace Mosa.Kernel.BareMetal.x86
{
	/// <summary>
	/// Page Table
	/// </summary>
	internal static class PageTable
	{
		public static IntPtr PageDirectory;
		public static IntPtr PageTables;

		public static void Setup()
		{
			PageDirectory = PhysicalPageAllocator.ReservePages(1);
			PageTables = PhysicalPageAllocator.ReservePages(Page.Size * 1024);
		}

		public static void Initialize()
		{
			// Setup Page Directory
			for (uint index = 0; index < 1024; index++)
			{
				PageDirectory.Store32(index << 2, (uint)(PageTables.ToInt32() + (index * 4096) | 0x04 | 0x02 | 0x01));
			}

			// Clear the Page Tables
			for (uint index = 0; index < 1024; index++)
			{
				Page.ClearPage(PageTables.Add(index * Page.Size));
			}
		}

		public static void Enable()
		{
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

		public static IntPtr GetPhysicalAddressFromVirtual(IntPtr virtualAddress)
		{
			//FUTURE: traverse page directory from CR3 --- do not assume page table is linearly allocated
			var offset = (((uint)virtualAddress.ToInt32() & 0xFFFFF000u) >> 10) + ((uint)virtualAddress.ToInt32() & 0xFFFu);

			return PageTables.LoadPointer(offset);
		}
	}
}
