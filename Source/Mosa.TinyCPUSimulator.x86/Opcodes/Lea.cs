// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.TinyCPUSimulator.x86.Opcodes
{
	public class Lea : BaseX86Opcode
	{
		public override void Execute(CPUx86 cpu, SimInstruction instruction)
		{
			uint address = GetAddress(cpu, instruction.Operand2);
			int size = instruction.Operand1.Size;

			StoreValue(cpu, instruction.Operand1, address, size);
		}
	}
}
