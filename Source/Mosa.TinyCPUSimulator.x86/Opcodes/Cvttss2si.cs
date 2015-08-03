// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.TinyCPUSimulator.x86.Opcodes
{
	public class Cvttss2si : BaseX86Opcode
	{
		public override void Execute(CPUx86 cpu, SimInstruction instruction)
		{
			var a = LoadFloatValue(cpu, instruction.Operand2, instruction.Operand2.Size);
			int size = instruction.Operand1.Size;

			uint r = (uint)a.LowF;
			StoreValue(cpu, instruction.Operand1, r, size);
		}
	}
}