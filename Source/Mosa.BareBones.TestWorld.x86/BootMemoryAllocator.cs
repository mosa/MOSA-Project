// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Runtime.x86;

namespace Mosa.BareBones.TestWorld.x86;

public static class BootMemoryAllocator
{
	public const uint StartAddress = 0x01000000;
	public const uint TotalSize = 0x8000000;

	public static uint UsedSize { get; private set; }

	public static void Setup() => UsedSize = 0;

	public static uint Allocate(uint size)
	{
		if (UsedSize + size <= TotalSize)
		{
			var address = StartAddress + UsedSize;
			UsedSize += size;
			return address;
		}

		// Out of memory
		Serial.Write(Serial.COM1, 0x45);

		for (; ; ) Native.Hlt();
	}
}
