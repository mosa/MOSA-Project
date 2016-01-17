// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.TinyCPUSimulator.x86.Opcodes
{
	public class Cvtsi2ss : BaseX86Opcode
	{
		public override void Execute(CPUx86 cpu, SimInstruction instruction)
		{
			int a = (int)LoadValue(cpu, instruction.Operand2);
			int size = instruction.Operand1.Size;

			StoreFloatValue(cpu, instruction.Operand1, a, size);
		}
	}
}
