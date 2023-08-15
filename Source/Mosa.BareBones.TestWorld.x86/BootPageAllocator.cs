// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Runtime.x86;

namespace Mosa.Demo.TestWorld.x86;

public static class BootPageAllocator
{
	public const uint StartAddress = 0x01000000;
	public const uint Size = 0x8000000;
	public const uint PageSize = 4096;
	public const uint TotalPages = Size / PageSize;

	public static uint UsedPages { get; private set; }

	public static void Setup() => UsedPages = 0;

	public static uint Reserve(uint pages)
	{
		if (UsedPages + pages <= TotalPages)
		{
			var address = StartAddress + UsedPages * PageSize;
			UsedPages += pages;
			return address;
		}

		// Out of memory
		Serial.Write(Serial.COM1, 0x45);

		for (; ; ) Native.Hlt();
	}
}
