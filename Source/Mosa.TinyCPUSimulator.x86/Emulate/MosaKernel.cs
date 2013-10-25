/*
 * (c) 2013 MOSA - The Managed Operating System Alliance
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

		public readonly uint GdtTable = 0x1401000;
		public readonly uint IdtTable = 0x1411000;
		public readonly uint PageTable = 1024 * 1024 * 16;
		public readonly uint PageDirectory = 1024 * 1024 * 20;
		public readonly uint PageFrameTable = 1024 * 1024 * 28;
		public readonly uint FreeMemory = 1024 * 1024 * 32; // start at 32Mb
		public readonly uint VirtualPageBitMap = 1024 * 1024 * 21; // 0x1500000

		public MosaKernel(SimCPU simCPU)
			: base(simCPU)
		{
		}

		public override void Initialize()
		{
			var x86 = simCPU as CPUx86;

			simCPU.AddMemory(PageDirectory, 1024 * 4, 1); // Page Directory (4K)
			simCPU.AddMemory(PageTable, 8192 * 16 * 4, 1); // Page Table (4Mb)

			simCPU.AddMemory(FreeMemory, 1024 * 1024 * 32, 1); // Free Memory (32Mb)

			simCPU.AddMemory(IdtTable, 256 * 256, 1); // IDT Table
			simCPU.AddMemory(GdtTable, 8 * 256, 1); // GDT Table
			simCPU.AddMemory(PageFrameTable, 1024 * 1024 * 4, 1); // Page Table (4Mb)
			simCPU.AddMemory(VirtualPageBitMap, 1024 * 1024, 1); // Virtual Page BitMap (4Mb)
		}

		public override void Reset()
		{
		}

		public override void MemoryWrite(ulong address, byte size)
		{
		}

		public override void PortWrite(uint port, byte value)
		{
		}

		public override byte PortRead(uint port)
		{
			return 0;
		}

		public override ushort[] GetPortList()
		{
			return null;
		}
	}
}