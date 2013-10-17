/*
 * (c) 2013 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System.Diagnostics;

namespace Mosa.TinyCPUSimulator.x86.Opcodes
{
	public class Div : BaseX86Opcode
	{
		public override void Execute(CPUx86 cpu, SimInstruction instruction)
		{
			// Divide EDX:EAX by the contents of a 32-bit register or memory location
			// and store the quotient in EAX and the remainder in EDX

			uint v1 = cpu.EDX.Value;
			uint v2 = cpu.EAX.Value;
			uint v3 = LoadValue(cpu, instruction.Operand1);
			int size = instruction.Operand1.Size;

			// TODO: for sizes other than 32
			Debug.Assert(size == 32);

			// TODO: exception if v3 is 0

			ulong v = (((ulong)v1 << 32) | v2);
			ulong a = v / (ulong)v3;
			ulong r = v % (ulong)v3;

			cpu.EAX.Value = (uint)a;
			cpu.EDX.Value = (uint)r;
		}
	}
}