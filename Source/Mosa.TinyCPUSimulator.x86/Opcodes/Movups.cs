// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.TinyCPUSimulator.x86.Opcodes
{
	public class Movups : BaseX86Opcode
	{
		public override void Execute(CPUx86 cpu, SimInstruction instruction)
		{
			var a = LoadFloatValue(cpu, instruction.Operand2, instruction.Size);
			int size = instruction.Size;

			StoreFloatValue(cpu, instruction.Operand1, a, size);
		}
	}
}
