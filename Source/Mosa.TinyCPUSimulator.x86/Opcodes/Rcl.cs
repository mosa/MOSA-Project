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
	public class Rcl : BaseX86Opcode
	{
		public override void Execute(CPUx86 cpu, SimInstruction instruction)
		{
			uint a = LoadValue(cpu, instruction.Operand1);
			uint b = LoadValue(cpu, instruction.Operand2);
			int size = instruction.Operand1.Size;

			int shift = ((int)b) & 0x1F;

			if (shift == 0)
				return; // no changes

			// TODO: for sizes other than 32
			Debug.Assert(size == 32);

			uint u = (a << 1);

			if (cpu.FLAGS.Carry)
				u = u | 0x1;

			shift--;

			u = u << shift;

			u = u | (a >> (size - shift));

			StoreValue(cpu, instruction.Operand1, (uint)u, size);

			cpu.FLAGS.Overflow = cpu.FLAGS.Carry ^ IsSign(a, size);
			cpu.FLAGS.Carry = ((a >> (size - shift)) & 0x1) == 1;
		}
	}
}