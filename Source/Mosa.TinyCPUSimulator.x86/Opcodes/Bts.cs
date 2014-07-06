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
	public class Bts : BaseX86Opcode
	{
		public override void Execute(CPUx86 cpu, SimInstruction instruction)
		{
			uint a = LoadValue(cpu, instruction.Operand1);
			uint b = LoadValue(cpu, instruction.Operand2);
			int size = instruction.Operand1.Size;
			int mod = instruction.Operand2.Size;

			bool c = (((a >> (int)b) & 0x01) == 1);

			uint u = a | (uint)(1 << (int)(b % mod));

			StoreValue(cpu, instruction.Operand1, (uint)u, size);

			cpu.EFLAGS.Carry = c;
		}
	}
}