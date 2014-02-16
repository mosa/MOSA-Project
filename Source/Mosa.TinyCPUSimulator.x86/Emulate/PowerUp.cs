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
	public class PowerUp : BaseSimDevice
	{
		public readonly uint VectorReset = 0xFFFFFFF0;
		public readonly string VectorCall = "@Linker.Default.StartUp()";

		public PowerUp(SimCPU simCPU)
			: base(simCPU)
		{
		}

		public override void Initialize()
		{
			var x86 = simCPU as CPUx86;

			simCPU.AddInstruction(VectorReset, new SimInstruction(Opcode.Call, 4, SimOperand.CreateLabel(32, VectorCall)));
		}

		public override void Reset()
		{
			var x86 = simCPU as CPUx86;

			// Start of stack
			x86.ESP.Value = 0x00080000;
			x86.EBP.Value = x86.ESP.Value;

			// Start EIP
			x86.EIP.Value = VectorReset;
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