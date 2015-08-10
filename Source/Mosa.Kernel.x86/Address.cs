// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Kernel.x86
{
	public static class Address
	{
		public const uint PageTable = 0x01000000; // 16MB
		public const uint PageDirectory = 0x01400000; // 20MB Offset 0KB

		public const uint GDTTable = 0x01401000; // 20MB Offset 1KB

		public const uint IDTTable = 0x01411000; // 20MB Offset 65KB

		public const uint TSS = 0x0; // TODO
		public const uint TSSSize = 0x68;

		public const uint VirtualPageAllocator = 0x01500000; // 21MB

		public const uint PageFrameAllocator = 0x01C00000; // 28MB

		public const uint ReserveMemory = 0x02000000; // 32MB

		public const uint MaximumMemory = 0xFFFFFFFF; // 4GB
	}
}
