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
	public class MosaTestMemory : BaseSimDevice
	{
		public static readonly uint BaseAddress = 0x00900000;
		public static readonly uint ImageSize = 0x40000000;

		public MosaTestMemory(SimCPU simCPU)
			: base(simCPU)
		{
		}

		public override void Initialize()
		{
			var x86 = simCPU as CPUx86;

			simCPU.AddMemory(BaseAddress, ImageSize, 1);
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