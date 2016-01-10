// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Kernel.x86.Smbios;
using Mosa.Platform.Internal.x86;
using Mosa.Internal;

namespace Mosa.Kernel.x86
{
	/// <summary>
	///
	/// </summary>
	public static class Kernel
	{
		public static void Setup()
		{
			// At this stage, allocating memory does not work, so you are only allowed to use ValueTypes or static classes.
			IDT.SetInterruptHandler(null);
			Panic.Setup();
			DebugClient.Setup(Serial.COM1);

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

			// At this point we can use objects, that allocates memory
			Mosa.Internal.Runtime.Setup();
			SmbiosManager.Setup();
			ConsoleManager.Setup();
		}
	}
}
