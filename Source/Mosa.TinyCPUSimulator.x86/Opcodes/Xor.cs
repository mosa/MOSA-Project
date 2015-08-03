// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.TinyCPUSimulator.x86.Opcodes
{
	public class Xor : BaseX86Opcode
	{
		public override void Execute(CPUx86 cpu, SimInstruction instruction)
		{
			uint a = LoadValue(cpu, instruction.Operand1);
			uint b = LoadValue(cpu, instruction.Operand2);
			int size = instruction.Operand2.Size;

			ulong u = a ^ b;
			long s = a ^ b;

			StoreValue(cpu, instruction.Operand1, (uint)u, size);

			UpdateFlags(cpu, size, s, u, true, true, true, false, false);

			cpu.EFLAGS.Overflow = false;
			cpu.EFLAGS.Carry = false;
		}
	}
}