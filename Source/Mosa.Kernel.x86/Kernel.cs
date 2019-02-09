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
			Logger.Log("Initialize Kernel");

			// Initialize GDT before IDT, because IDT Entries requies a valid Segment Selector
			// This never happend before, because on fast computers GDT.Setup() was called
			// before a Interrupt,for example clock, got triggered.
			Multiboot.Setup();
			GDT.Setup();

			// At this stage, allocating memory does not work, so you are only allowed to use ValueTypes or static classes.
			IDT.SetInterruptHandler(null);
			Panic.Setup();
			Debugger.Setup(Serial.COM2);

			// Initialize interrupts
			PIC.Setup();
			IDT.Setup();

			// Initializing the memory management
			PageFrameAllocator.Setup();
			PageTable.Setup();
			VirtualPageAllocator.Setup();
			GC.Setup();

			// At this point we can use objects
			Scheduler.Setup();
			SmbiosManager.Setup();
			ConsoleManager.Setup();

			Logger.Log("Kernel initialized");
		}
	}
}
