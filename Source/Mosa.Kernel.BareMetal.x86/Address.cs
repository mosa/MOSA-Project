// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Kernel.BareMetal.x86
{
	internal static class Address
	{
		public const uint MemoryMap = 0x00007E00;  // 1KB [Size=1KB]

		public const uint GDTTable = 0x00B10000;  // 12MB+ [Size=1KB]
		public const uint IDTTable = 0x00B11000;  // 12MB+ [Size=1KB]

		public const uint PageDirectory = 0x00B00000;  // 12MB [Size=4KB]
		public const uint PageTables = 0x01000000;  // 16MB [Size=4MB]

		public const uint GCInitialMemory = 0x03000000;  // 48MB [Size=16MB]

		public const uint InitialStack = 0x000F0000; // ???KB (stack grows down)
	}
}
