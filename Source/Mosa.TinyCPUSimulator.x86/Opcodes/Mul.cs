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
	public class Mul : BaseX86Opcode
	{
		public override void Execute(CPUx86 cpu, SimInstruction instruction)
		{
			ulong a = cpu.EAX.Value;
			ulong b = LoadValue(cpu, instruction.Operand1);
			int size = instruction.Operand1.Size;

			ulong r = a * b;

			cpu.EAX.Value = (uint)r;
			cpu.EDX.Value = (uint)(r >> 32);

			cpu.FLAGS.Overflow = ((r >> 32) != 0);
			cpu.FLAGS.Carry = ((r >> 32) != 0);
		}
	}
}