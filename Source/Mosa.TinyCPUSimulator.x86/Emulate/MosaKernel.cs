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