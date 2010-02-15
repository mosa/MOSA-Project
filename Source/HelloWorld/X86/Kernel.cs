/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.Kernel.X86;

namespace Mosa.HelloWorld
{
	/// <summary>
	/// 
	/// </summary>
	public static class Kernel
	{
		private static uint _multibootptr = 0x200004;
		private static uint _multibootsignature = 0x200000;

		public static void Setup()
		{
			Screen.Clear();
			Screen.Color = 0x0E;

			Screen.SetCursor(24, 0);
			Screen.Write('1');
			Multiboot.SetMultibootLocation(Memory.Get32(_multibootptr), Memory.Get32(_multibootsignature));
			Screen.SetCursor(24, 1);
			Screen.Write('2');
			Screen.SetCursor(24, 2);

			if (Multiboot.IsMultibootEnabled) 
				Screen.Write('3');			
			else
				Screen.Write('*');	// Panic! 

			PIC.Setup();
			Screen.SetCursor(24, 3);
			Screen.Write('4');
			GDT.Setup();
			Screen.SetCursor(24, 4);
			Screen.Write('5');
			IDT.Setup();
			Screen.SetCursor(24, 5);
			Screen.Write('6');
			PageFrameAllocator.Setup();
			Screen.SetCursor(24, 6);
			Screen.Write('7');
			PageTable.Setup();
			Screen.SetCursor(24, 6);
			Screen.Write('8');
		}

		private static uint _processtable = 0x1000;
		private static uint _threadtable = 0x2000;

		public static void Test()
		{
			uint page1 = PageFrameAllocator.Allocate();
			uint page2 = PageFrameAllocator.Allocate();

			PageTable.MapVirtualAddressToPhysical(_processtable, page1);
			PageTable.MapVirtualAddressToPhysical(_threadtable, page2);

			Memory.Clear(_processtable, PageFrameAllocator.PageSize);
			Memory.Clear(_threadtable, PageFrameAllocator.PageSize);

			Memory.Set8(page1, 0xAA);
			Memory.Set8(page2, 0xBB);
		}

	}
}
