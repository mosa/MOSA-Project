// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.TinyCPUSimulator.x86.Opcodes
{
	public class Divsd : BaseX86Opcode
	{
		public override void Execute(CPUx86 cpu, SimInstruction instruction)
		{
			var a = LoadFloatValue(cpu, instruction.Operand1, instruction.Size);
			var b = LoadFloatValue(cpu, instruction.Operand2, instruction.Size);
			int size = instruction.Size;

			double r = a.Low / b.Low;

			StoreFloatValue(cpu, instruction.Operand1, r, size);
		}
	}
}
