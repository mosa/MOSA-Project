// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.TinyCPUSimulator.x86.Opcodes
{
	public class Addss : BaseX86Opcode
	{
		public override void Execute(CPUx86 cpu, SimInstruction instruction)
		{
			var a = LoadFloatValue(cpu, instruction.Operand1, instruction.Size);
			var b = LoadFloatValue(cpu, instruction.Operand2, instruction.Size);
			int size = instruction.Size;

			float r = a.LowF + b.LowF;

			StoreFloatValue(cpu, instruction.Operand1, r, size);
		}
	}
}
