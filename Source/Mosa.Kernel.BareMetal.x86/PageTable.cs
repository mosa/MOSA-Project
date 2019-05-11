// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Kernel.BareMetal.Extension;
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
			PageDirectory = new IntPtr(Address.PageDirectory); // 12MB [Size=4KB]
			PageTables = new IntPtr(Address.PageTables); // 16MB [Size=4MB]
		}

		public static void Initialize()
		{
			// Setup Page Directory
			for (int index = 0; index < 1024; index++)
			{
				PageDirectory.Store32(index << 2, (uint)(PageTables.ToInt32() + (index * 4096) | 0x04 | 0x02 | 0x01));
			}

			// TODO: Clear all the page table entries

			// Set CR3 register on processor - sets page directory
			Native.SetCR3((uint)PageDirectory.ToInt32());

			// Set CR0 register on processor - turns on virtual memory
			Native.SetCR0(Native.GetCR0() | 0x80000000);
		}

		public static void MapVirtualAddressToPhysical(uint virtualAddress, uint physicalAddress, bool present = true)
		{
			//FUTURE: traverse page directory from CR3 --- do not assume page table is linearly allocated

			//Native.Set32(Address.PageTable + ((virtualAddress & 0xFFC00000u) >> 10), physicalAddress & 0xFFC00000u | 0x04u | 0x02u | (present ? 0x1u : 0x0u));
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
