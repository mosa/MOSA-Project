// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.TinyCPUSimulator.x86.Opcodes
{
	public class Neg : BaseX86Opcode
	{
		public override void Execute(CPUx86 cpu, SimInstruction instruction)
		{
			uint a = LoadValue(cpu, instruction.Operand1);
			int size = instruction.Operand1.Size;

			uint u = 0 - a;

			StoreValue(cpu, instruction.Operand1, u, size);

			UpdateFlags(cpu, size, u, u, true, true, true, false, false);

			cpu.EFLAGS.Carry = !cpu.EFLAGS.Zero;
		}
	}
}
