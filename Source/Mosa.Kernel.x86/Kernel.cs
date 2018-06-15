// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Kernel.x86.Smbios;
using Mosa.Runtime;

namespace Mosa.Kernel.x86
{
	/// <summary>
	/// X86 Kernel
	/// </summary>
	public static class Kernel
	{
		public static void Setup()
		{
			// At this stage, allocating memory does not work, so you are only allowed to use ValueTypes or static classes.
			IDT.SetInterruptHandler(null);
			Panic.Setup();
			Debugger.Setup(Serial.COM1);

			// Initialize interrupts
			PIC.Setup();
			IDT.Setup();

			// Initializing the memory management
			Multiboot.Setup();
			GDT.Setup();
			PageFrameAllocator.Setup();
			PageTable.Setup();
			VirtualPageAllocator.Setup();
			GC.Setup();

			// At this point we can use objects
			Scheduler.Setup();
			SmbiosManager.Setup();
			ConsoleManager.Setup();
			Internal.Setup();
		}
	}
}
