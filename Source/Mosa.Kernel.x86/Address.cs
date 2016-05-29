// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Kernel.x86
{
	public static class Address
	{
		public const uint PageDirectory = 0x00200000; // 2MB Offset 0KB - Size 64KB
		public const uint GDTTable = 0x00210000; // 2MB Offset 64KB - Size 64KB
		public const uint IDTTable = 0x00220000; // 2MB Offset 128KB - Size 64KB
		public const uint ProcessSlots = 0x00230000; // 2MB Offset 192KB - Size 64KB
		public const uint TaskSlots = 0x00240000; // 2MB Offset 256KB - Size 320KB

		public const uint PageFrameAllocator = 0x00C00000; // 12MB
		public const uint PageTable = 0x01000000; // 16MB

		public const uint TSS = 0x0; // TODO
		public const uint TSSSize = 0x68;

		public const uint VirtualPageAllocator = 0x01400000; // 20MB

		public const uint GCInitialMemory = 0x02000000; // 8MB

		public const uint ReserveMemory = 0x03000000; // 48MB
		public const uint MaximumMemory = 0xFFFFFFFF; // 4GB

		public const uint UnitTestStack = 0x00001000;   // 4K
		public const uint DebuggerBuffer = 0x00004000;  // 16k
	}
}
