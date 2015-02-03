/*
 * (c) 2014 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

namespace Mosa.TinyCPUSimulator.x86.Emulate
{
	public class MosaKernel : BaseSimDevice
	{
		public static readonly uint GdtTable = 0x1401000;
		public static readonly uint IdtTable = 0x1411000;
		public static readonly uint PageTable = 1024 * 1024 * 16;
		public static readonly uint PageDirectory = 1024 * 1024 * 20;
		public static readonly uint PageFrameTable = 1024 * 1024 * 28;
		public static readonly uint FreeMemory = 1024 * 1024 * 32; // start at 32Mb
		public static readonly uint VirtualPageBitMap = 1024 * 1024 * 21; // 0x1500000

		public MosaKernel(SimCPU simCPU)
			: base(simCPU)
		{
		}

		public override void Initialize()
		{
			var x86 = simCPU as CPUx86;

			simCPU.AddMemory(PageDirectory, 1024 * 4, 1); // Page Directory (4K)
			simCPU.AddMemory(PageTable, 8192 * 16 * 4, 1); // Page Table (4Mb)
			simCPU.AddMemory(FreeMemory, 1024 * 1024 * 64, 1); // Free Memory (64Mb)
			simCPU.AddMemory(IdtTable, 256 * 256, 1); // IDT Table
			simCPU.AddMemory(GdtTable, 8 * 256, 1); // GDT Table
			simCPU.AddMemory(PageFrameTable, 1024 * 1024 * 4, 1); // Page Table (4Mb)
			simCPU.AddMemory(VirtualPageBitMap, 1024 * 1024, 1); // Virtual Page BitMap (4Mb)
		}
	}
}