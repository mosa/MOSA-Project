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
	public class And : BaseX86Opcode
	{
		public override void Execute(CPUx86 cpu, SimInstruction instruction)
		{
			uint a = LoadValue(cpu, instruction.Operand1);
			uint b = LoadValue(cpu, instruction.Operand2);
			int size = instruction.Operand2.Size;

			uint r = a & b;

			StoreValue(cpu, instruction.Operand1, (uint)r, size);

			cpu.FLAGS.Zero = IsZero(r, size);
			cpu.FLAGS.Sign = IsSign(r, size);
			cpu.FLAGS.Parity = IsParity(r);
			cpu.FLAGS.Overflow = false;
			cpu.FLAGS.Carry = false;
		}
	}
}