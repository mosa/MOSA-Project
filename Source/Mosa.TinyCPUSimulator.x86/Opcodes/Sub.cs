// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.TinyCPUSimulator.x86.Opcodes
{
	public class Sub : BaseX86Opcode
	{
		public override void Execute(CPUx86 cpu, SimInstruction instruction)
		{
			uint a = LoadValue(cpu, instruction.Operand1);
			uint b = LoadValue(cpu, instruction.Operand2);
			int size = instruction.Operand2.Size;

			long s = (long)(int)a - (long)(int)b;
			ulong u = (ulong)a - (ulong)b;

			StoreValue(cpu, instruction.Operand1, (uint)u, size);

			UpdateFlags(cpu, size, s, u, true, true, true, true, true);

			cpu.EFLAGS.Adjust = IsAdjustAfterSub(a, b);
		}
	}
}