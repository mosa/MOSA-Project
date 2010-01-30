/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

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
		public static void Start()
		{
			// Clear Screen
			//Screen.Clear();
			//Screen.SetCursor(0, 0);
			//Screen.Color = 0x0E;

			// Set Multiboot Structure Location
			//Multiboot.SetMultibootLocation(Memory.Get32(0x200004), Memory.Get32(0x200000));
			
			// Setup Page Frame Allocator
			//PageFrameAllocator.Setup();

			// Setup Page Table
			//PageTable.Setup();

			// Setup Stack
			// Setup Global Descriptor Table (GDT)
			// Setup Interrupt Descriptor Table (IDT)
			// and much more...
		}
	}

}

