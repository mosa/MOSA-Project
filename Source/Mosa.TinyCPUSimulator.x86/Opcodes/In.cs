// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.TinyCPUSimulator.x86.Opcodes
{
	public class In : BaseX86Opcode
	{
		public override void Execute(CPUx86 cpu, SimInstruction instruction)
		{
			uint a = LoadValue(cpu, instruction.Operand2);
			int size = instruction.Operand1.Size;

			uint value = 0;

			if (size == 8)
				value = cpu.Read8Port(a);
			else if (size == 16)
				value = cpu.Read16Port(a);
			else if (size == 32)
				value = cpu.Read32Port(a);

			StoreValue(cpu, instruction.Operand1, (uint)value, size);
		}
	}
}
