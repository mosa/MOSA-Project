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
	public class Shld : BaseX86Opcode
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

			uint r = (a << shift) | (b >> (size - shift));

			StoreValue(cpu, instruction.Operand1, (uint)r, size);

			cpu.FLAGS.Overflow = IsSign(a, size) != IsSign(r, size);
			cpu.FLAGS.Carry = ((a >> (shift - 1)) & 0x1) == 1;
			cpu.FLAGS.Zero = IsZero(r, size);
			cpu.FLAGS.Sign = IsSign(r, size);
			cpu.FLAGS.Parity = IsParity(r);
		}
	}
}