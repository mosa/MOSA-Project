/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.Platforms.x86;

namespace Mosa.Kernel.X86
{
	/// <summary>
	/// 
	/// </summary>
	public static class PageTable
	{
		// Location for page directory starts at 20MB
		private static uint _pageDirectory = 1024 * 1024 * 20; // 0x1400000

		// Location for page tables start at 16MB
		private static uint _pageTable = 1024 * 1024 * 16;	// 0x1000000

		/// <summary>
		/// Sets up the PageTable
		/// </summary>
		public static void Setup()
		{
			// Setup Page Directory
			for (int index = 0; index < 1024; index++)
				Memory.Set32((uint)(_pageDirectory + (index * 4)), (uint)(_pageTable + (index * 4096) | 0x04 | 0x02 | 0x01));

			// Map the first 32MB of memory (8192 4K pages)
			for (int index = 0; index < 8192 * 16; index++) // FIXME: It's not 32MB
				Memory.Set32((uint)(_pageTable + (index * 4)), (uint)(index * 4096) | 0x04 | 0x02 | 0x01);

			// Set CR3 register on processor - sets page directory
			Native.SetControlRegister(3, _pageDirectory);

			// Set CR0 register on processor - turns on virtual memory
			Native.SetControlRegister(0, Native.GetControlRegister(0) | 0x80000000);
		}

		/// <summary>
		/// Maps the virtual address to physical.
		/// </summary>
		/// <param name="virtualAddress">The virtual address.</param>
		/// <param name="physicalAddress">The physical address.</param>
		public static void MapVirtualAddressToPhysical(uint virtualAddress, uint physicalAddress)
		{
			Memory.Set32(_pageTable + ((virtualAddress >> 12) * 4), (uint)(physicalAddress | 0x04 | 0x02 | 0x01));
		}
	}
}
