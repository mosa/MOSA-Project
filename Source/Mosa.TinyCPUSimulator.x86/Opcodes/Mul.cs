// Copyright (c) MOSA Project. Licensed under the New BSD License.

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

			cpu.EFLAGS.Overflow = ((r >> 32) != 0);
			cpu.EFLAGS.Carry = ((r >> 32) != 0);
		}
	}
}