// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;

namespace Mosa.Kernel.BareMetal
{
	/// <summary>
	/// Page Table
	/// </summary>
	public static class PageTable
	{
		public static void Setup()
		{
			Platform.PageTableSetup();

			Platform.PageTableInitialize();

			// Unmap the first page for null pointer exceptions
			MapVirtualAddressToPhysical(0x0, 0x0, false);
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
		public static IntPtr GetPhysicalAddressFromVirtual(IntPtr virtualAddress)
		{
			return IntPtr.Zero;
		}
	}
}
