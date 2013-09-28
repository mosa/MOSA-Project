/*
 * (c) 2013 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

namespace Mosa.TinyCPUSimulator.x86.Opcodes
{
	public class Shl : BaseX86Opcode
	{
		public override void Execute(CPUx86 cpu, SimInstruction instruction)
		{
			uint a = LoadValue(cpu, instruction.Operand1);
			uint b = LoadValue(cpu, instruction.Operand2);
			int size = instruction.Operand1.Size;

			int shift = ((int)b) & 0x1F;

			if (shift == 0)
				return; // no changes

			uint u = a << shift;
			bool sign = IsSign(a, size);

			if (cpu.FLAGS.Carry)
			{
				u = u | 0x1;
			}

			StoreValue(cpu, instruction.Operand1, (uint)u, size);

			UpdateFlags(cpu, size, (long)u, u, true, true, true, false, false);

			cpu.FLAGS.Overflow = sign ^ IsSign(u, size);
			cpu.FLAGS.Carry = sign;
		}
	}
}