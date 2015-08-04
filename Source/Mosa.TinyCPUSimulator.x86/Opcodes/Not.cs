// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.TinyCPUSimulator.x86.Opcodes
{
	public class Not : BaseX86Opcode
	{
		public override void Execute(CPUx86 cpu, SimInstruction instruction)
		{
			uint a = LoadValue(cpu, instruction.Operand1);
			int size = instruction.Operand1.Size;

			uint u = ~a;

			StoreValue(cpu, instruction.Operand1, u, size);
		}
	}
}
