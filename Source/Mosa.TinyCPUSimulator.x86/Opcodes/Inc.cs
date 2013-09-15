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
	public class Inc : BaseX86Opcode
	{
		public override void Execute(CPUx86 cpu, SimInstruction instruction)
		{
			uint a = LoadValue(cpu, instruction.Operand1);
			int size = instruction.Operand2.Size;

			ulong r = a++;

			StoreValue(cpu, instruction.Operand1, (uint)r, size);

			cpu.FLAGS.Zero = IsZero(r, size);
			cpu.FLAGS.Sign = IsSign(r, size);
			cpu.FLAGS.Parity = IsParity(r);
			cpu.FLAGS.Adjust = IsAdjustAfterAdd(a, 1);
			cpu.FLAGS.Overflow = !IsSign(a, size) && IsSign(r, size);
		}
	}
}