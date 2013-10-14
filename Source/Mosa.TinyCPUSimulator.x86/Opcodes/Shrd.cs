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
	public class Shrd : BaseX86Opcode
	{
		public override void Execute(CPUx86 cpu, SimInstruction instruction)
		{
			uint a = LoadValue(cpu, instruction.Operand1);
			uint b = LoadValue(cpu, instruction.Operand2);
			uint c = LoadValue(cpu, instruction.Operand3);
			int size = instruction.Operand1.Size;

			int shift = ((int)c) & 0x1F;

			if (shift == 0)
				return; // no changes

			uint u = (a >> shift) | (b << (size - shift));

			StoreValue(cpu, instruction.Operand1, (uint)u, size);

			UpdateFlags(cpu, size, (long)u, u, true, true, true, false, false);

			cpu.FLAGS.Overflow = IsSign(a, size) != IsSign(u, size);
			cpu.FLAGS.Carry = ((a >> (shift - 1)) & 0x1) == 1;
		}
	}
}