// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.TinyCPUSimulator.x86.Opcodes
{
	public class Shr : BaseX86Opcode
	{
		public override void Execute(CPUx86 cpu, SimInstruction instruction)
		{
			uint a = LoadValue(cpu, instruction.Operand1);
			uint b = LoadValue(cpu, instruction.Operand2);
			int size = instruction.Operand1.Size;

			int shift = ((int)b) & 0x1F;

			if (shift == 0)
				return; // no changes

			uint u = a >> shift;

			StoreValue(cpu, instruction.Operand1, u, size);

			UpdateFlags(cpu, size, u, u, true, true, false, false, false);

			cpu.EFLAGS.Overflow = IsSign(a, size);
			cpu.EFLAGS.Carry = ((a >> (shift - 1)) & 0x1) == 1;
			cpu.EFLAGS.Sign = false;
		}
	}
}
