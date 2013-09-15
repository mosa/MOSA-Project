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
	public class Sbb : BaseX86Opcode
	{
		public override void Execute(CPUx86 cpu, SimInstruction instruction)
		{
			uint a = LoadValue(cpu, instruction.Operand1);
			uint b = LoadValue(cpu, instruction.Operand2);
			int size = instruction.Operand2.Size;

			uint r = a - b;

			if (cpu.FLAGS.Carry)
				r--;

			StoreValue(cpu, instruction.Operand1, (uint)r, size);

			cpu.FLAGS.Overflow = IsOverflow(r, a, b, size);
			cpu.FLAGS.Carry = ((b + 1) > a);

			cpu.FLAGS.Zero = IsZero(r, size);
			cpu.FLAGS.Sign = IsSign(r, size);
			cpu.FLAGS.Parity = IsParity(r);
			cpu.FLAGS.Adjust = IsAdjustAfterSub(a, b);
		}
	}
}