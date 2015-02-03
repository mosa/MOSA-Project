/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.Platform.Internal.x86;

namespace Mosa.Kernel.x86
{
	/// <summary>
	///
	/// </summary>
	public static class PageTable
	{
		// Location for page directory starts at 20MB
		private static uint pageDirectory = 1024 * 1024 * 20; // 0x1400000

		// Location for page tables start at 16MB
		private static uint pageTable = 1024 * 1024 * 16;	// 0x1000000

		/// <summary>
		/// Sets up the PageTable
		/// </summary>
		public static void Setup()
		{
			// Setup Page Directory
			for (int index = 0; index < 1024; index++)
			{
				Native.Set32((uint)(pageDirectory + (index << 2)), (uint)(pageTable + (index * 4096) | 0x04 | 0x02 | 0x01));
			}

			// Map the first 256MB of memory (65536 4K pages) (why 256MB?)
			for (int index = 0; index < 1024 * 64; index++)
			{
				Native.Set32((uint)(pageTable + (index << 2)), (uint)(index * 4096) | 0x04 | 0x02 | 0x01);
			}

			// Set CR3 register on processor - sets page directory
			Native.SetCR3(pageDirectory);

			// Set CR0 register on processor - turns on virtual memory
			Native.SetCR0(Native.GetCR0() | 0x80000000);
		}

		/// <summary>
		/// Maps the virtual address to physical.
		/// </summary>
		/// <param name="virtualAddress">The virtual address.</param>
		/// <param name="physicalAddress">The physical address.</param>
		public static void MapVirtualAddressToPhysical(uint virtualAddress, uint physicalAddress)
		{
			//Panic.Now(virtualAddress);
			//FUTURE: traverse page directory from CR3 --- do not assume page table is linearly allocated
			uint pdIndex = virtualAddress >> 22;
			uint ptIndex = virtualAddress >> 12 & 0x03FF;
			uint location = pageTable + (0x1000 * pdIndex) + ptIndex;
			Native.Set32(location, (uint)(physicalAddress & 0xFFC00000 | 0x04 | 0x02 | 0x01));
			// Flush TLB
			Native.SetCR3(Native.GetCR3());
		}

		/// <summary>
		/// Gets the physical memory.
		/// </summary>
		/// <param name="virtualAddress">The virtual address.</param>
		/// <returns></returns>
		public static uint GetPhysicalAddressFromVirtual(uint virtualAddress)
		{
			//FUTURE: traverse page directory from CR3 --- do not assume page table is linearly allocated
			uint pdIndex = virtualAddress >> 22;
			uint ptIndex = virtualAddress >> 12 & 0x03FF;
			uint location = pageTable + (0x400 * pdIndex) + ptIndex;
			return Native.Get32(location + (virtualAddress & 0xFFF));
		}
	}
}
