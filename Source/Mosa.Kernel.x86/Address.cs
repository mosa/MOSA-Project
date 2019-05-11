// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Kernel.x86
{
	public static class Address
	{
		public const uint InitialStack = 0x000F0000; // ???KB (stack grows down)

		public const uint PageDirectory = 0x00B00000;  // 12MB [Size=4KB]
		public const uint GDTTable = 0x00B10000;  // 12MB+ [Size=1KB]
		public const uint IDTTable = 0x00B11000;  // 12MB+ [Size=1KB]

		public const uint PageFrameAllocator = 0x00C00000;  // 13MB [Size=4MB]
		public const uint PageTable = 0x01000000;  // 16MB [Size=4MB]
		public const uint VirtualPageAllocator = 0x01400000;  // 20MB [Size=32KB]

		public const uint GCInitialMemory = 0x03000000;  // 48MB [Size=16MB]

		public const uint ReserveMemory = 0x05000000;  // 64MB
		public const uint MaximumMemory = 0xFFFFFFFF;  // 4GB

		public const uint UnitTestStack = 0x00004000;  // 4KB (stack grows down)
		public const uint UnitTestQueue = 0x01E00000;  // 30MB [Size=2MB] - previous: 5KB [Size=1KB] 0x00005000
		public const uint DebuggerBuffer = 0x00010000;  // 16KB [Size=64KB]
	}
}
