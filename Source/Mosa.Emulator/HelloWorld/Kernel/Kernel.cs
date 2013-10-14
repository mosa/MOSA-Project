/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.Kernel.x86.Smbios;

namespace Mosa.Kernel.x86
{
	/// <summary>
	///
	/// </summary>
	public static class Kernel
	{
		public static void Setup()
		{
			SmbiosManager.Setup();
			Screen.Clear();
			Screen.Color = 0x0E;
			Screen.Goto(24, 0);

			//Screen.Write('1');
			Multiboot.Setup();
			Screen.Goto(24, 1);

			//Screen.Write('2');
			PIC.Setup();
			Screen.Goto(24, 2);

			//Screen.Write('3');
			GDT.Setup();
			Screen.Goto(24, 3);

			//Screen.Write('4');
			IDT.Setup();
			Screen.Goto(24, 4);

			//Screen.Write('5');
			PageFrameAllocator.Setup();
			Screen.Goto(24, 5);

			//Screen.Write('6');
			PageTable.Setup();
			Screen.Goto(24, 6);

			//Screen.Write('7');
			VirtualPageAllocator.Setup();
			Screen.Goto(24, 7);

			//Screen.Write('8');
			Screen.Goto(24, 8);
			ProcessManager.Setup();

			//Screen.Write('9');
			Screen.Goto(24, 9);
			TaskManager.Setup();

			//Screen.Write('A');
			Screen.Goto(24, 10);
			SmbiosManager.Setup();
		}
	}
}