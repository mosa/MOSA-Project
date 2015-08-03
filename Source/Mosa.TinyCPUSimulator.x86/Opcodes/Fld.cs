// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.TinyCPUSimulator.x86.Opcodes
{
	public class Fld : BaseX86Opcode
	{
		public override void Execute(CPUx86 cpu, SimInstruction instruction)
		{
			var r = LoadFloatValue(cpu, instruction.Operand1, instruction.Operand1.Size);
			int size = instruction.Operand1.Size;

			cpu.ST0.Value = r;
		}
	}
}