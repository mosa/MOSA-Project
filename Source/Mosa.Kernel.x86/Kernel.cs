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
			Multiboot.Setup();
			ProgrammableInterruptController.Setup();
			GDT.Setup();
			IDT.Setup();
			PageFrameAllocator.Setup();
			PageTable.Setup();
			VirtualPageAllocator.Setup();
			ProcessManager.Setup();
			TaskManager.Setup();
			SmbiosManager.Setup();
			ConsoleManager.Setup();
		}
	}
}
