// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Runtime;
using Mosa.Runtime.Plug;
using Mosa.Runtime.x86;

namespace Mosa.BareBones.TestWorld.x86;

public static class Boot
{
	[Plug("Mosa.Runtime.StartUp::PlatformInitialization")]
	public static void PlatformInitialization()
	{
		// Initialize the GDT, IDT, etc... stuff here, as well as the GC
		Serial.Setup(Serial.COM1);
		Serial.Write(Serial.COM1, 0x41);

		BootPageAllocator.Setup();
	}

	[Plug("Mosa.Runtime.Interrupt::Process")]
	public static void Process()
	{
		// Put your interrupt handler here
		Serial.Write(Serial.COM1, 0x42);
	}

	[Plug("Mosa.Runtime.GC::AllocateMemory")]
	public static Pointer AllocateMemory(uint size)
	{
		// Call to your memory allocator here
		Serial.Write(Serial.COM1, 0x43);

		var alignedSize = size;
		while (alignedSize % BootPageAllocator.PageSize != 0) alignedSize++;

		return new Pointer(BootPageAllocator.Reserve(alignedSize / BootPageAllocator.PageSize));
	}

	public static void Main()
	{
		Serial.Write(Serial.COM1, 0x44);

		for (; ; ) Native.Hlt();
	}
}
