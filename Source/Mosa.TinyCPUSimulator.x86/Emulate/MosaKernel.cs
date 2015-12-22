// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.TinyCPUSimulator.x86.Emulate
{
	public class MosaKernel : BaseSimDevice
	{
		public static readonly uint SmallTables = 1024 * 1024 * 2; // Includes GDT, IDT, Page Directory, etc
		public static readonly uint KernelStack = 1024 * 1024 * 3;
		public static readonly uint PageTable = 1024 * 1024 * 16;
		public static readonly uint PageFrameTable = 1024 * 1024 * 12;
		public static readonly uint FreeMemory = 1024 * 1024 * 32; // start at 32MB
		public static readonly uint VirtualPageBitMap = 1024 * 1024 * 20; // 0x01400000

		public MosaKernel(SimCPU simCPU)
			: base(simCPU)
		{
		}

		public override void Initialize()
		{
			var x86 = simCPU as CPUx86;

			simCPU.AddMemory(SmallTables, 1024 * 1024 * 1, 1); // Small Tables (1MB)
			simCPU.AddMemory(KernelStack, 1024 * 1024 * 1, 1); // Kernel Stack (1MB)
			simCPU.AddMemory(PageTable, 1024 * 1024 * 4, 1); // Page Table (4MB)
			simCPU.AddMemory(FreeMemory, 1024 * 1024 * 64, 1); // Free Memory (64MB)
			simCPU.AddMemory(PageFrameTable, 1024 * 1024 * 4, 1); // Page Table (4MB)
			simCPU.AddMemory(VirtualPageBitMap, 1024 * 1024 * 4, 1); // Virtual Page BitMap (4MB)
		}

		public override BaseSimDevice Clone(SimCPU simCPU)
		{
			return new MosaKernel(simCPU);
		}
	}
}
