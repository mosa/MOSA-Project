/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.Kernel.Memory.X86;

namespace Mosa.Kernel
{
	/// <summary>
	/// 
	/// </summary>
	public static class Boot
	{

		/// <summary>
		/// Entry point for the Kernel 
		/// </summary>
		public static void Start(uint eax, uint ebx)
		{
			// Set Multiboot Structure Location
			Multiboot.SetMultibootLocation(ebx, eax); 

			// Setup Page Frame Allocator
			PageFrameAllocator.Setup();

			// Setup Page Table
			PageTable.Setup();

			// Setup Stack
			// Setup Global Descriptor Table (GDT)
			// Setup Interrupt Descriptor Table (IDT)
			// and much more...
		}
	}

}

