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
	public class Add : BaseX86Opcode
	{
		public override void Execute(CPUx86 cpu, SimInstruction instruction)
		{
			uint a = LoadValue(cpu, instruction.Operand1);
			uint b = LoadValue(cpu, instruction.Operand2);
			int size = instruction.Operand2.Size;

			ulong r = (ulong)a + (ulong)b;

			StoreValue(cpu, instruction.Operand1, (uint)r, size);

			cpu.FLAGS.Zero = IsZero(r, size);
			cpu.FLAGS.Sign = IsSign(r, size);
			cpu.FLAGS.Parity = IsParity(r);
			cpu.FLAGS.Adjust = IsAdjustAfterAdd(a, b);
			cpu.FLAGS.Overflow = IsOverflow(r, a, b, size);

			if (size == 32) cpu.FLAGS.Carry = ((r >> 32) != 0);
			else if (size == 16) cpu.FLAGS.Carry = ((r >> 16) != 0);
			else if (size == 8) cpu.FLAGS.Carry = ((r >> 8) != 0);
		}
	}
}