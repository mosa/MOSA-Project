/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.Kernel.x86.Smbios;
using Mosa.Platform.Internal.x86;

namespace Mosa.Kernel.x86
{
	/// <summary>
	///
	/// </summary>
	public static class Kernel
	{
		public static void Setup()
		{
			//At this stage, allocating memory does not work, so you are only allowed to use ValueTypes or static classes.
			IDT.SetInterruptHandler(null);
			Panic.Setup();
			DebugClient.Setup(Serial.COM1);
			SSE.Setup();

			//Initialize interrupts
			PIC.Setup();
			IDT.Setup();

			//Initializing the memory management
			Multiboot.Setup();
			GDT.Setup();
			PageFrameAllocator.Setup();
			PageTable.Setup();
			VirtualPageAllocator.Setup();

			//At this point we can use objects, that allocates memory
			Runtime.Setup();
			ProcessManager.Setup();
			TaskManager.Setup();
			SmbiosManager.Setup();
			ConsoleManager.Setup();
		}
	}
}