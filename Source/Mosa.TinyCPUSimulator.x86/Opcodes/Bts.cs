// Copyright (c) MOSA Project. Licensed under the New BSD License.

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

			StoreValue(cpu, instruction.Operand1, u, size);

			cpu.EFLAGS.Carry = c;
		}
	}
}
