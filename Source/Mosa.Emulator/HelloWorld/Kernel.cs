/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.Kernel.x86;

namespace Mosa.HelloWorld
{
	/// <summary>
	/// 
	/// </summary>
	public static class Kernel
	{

		public static void Setup()
		{
			Screen.Clear();
			Screen.Color = 0x0E;
			Screen.SetCursor(24, 0);
			Screen.Write('1');
			Multiboot.Setup();
			Screen.SetCursor(24, 1);
			Screen.Write('2');
			PIC.Setup();
			Screen.SetCursor(24, 2);
			Screen.Write('3');
			GDT.Setup();
			Screen.SetCursor(24, 3);
			Screen.Write('4');
			IDT.Setup();
			Screen.SetCursor(24, 4);
			Screen.Write('5');
			PageFrameAllocator.Setup();
			Screen.SetCursor(24, 5);
			Screen.Write('6');
			PageTable.Setup();
			Screen.SetCursor(24, 6);
			Screen.Write('7');
			VirtualPageAllocator.Setup();
			Screen.SetCursor(24, 7);
			Screen.Write('8');
			Screen.SetCursor(24, 8);
			ProcessManager.Setup();
			Screen.Write('9');
			Screen.SetCursor(24, 9);
			TaskManager.Setup();
			Screen.Write('A');
			Screen.SetCursor(24, 10);
		}
	}
}
