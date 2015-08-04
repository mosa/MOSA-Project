// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.TinyCPUSimulator.x86.Opcodes
{
	public class Push : BaseX86Opcode
	{
		public override void Execute(CPUx86 cpu, SimInstruction instruction)
		{
			uint a = LoadValue(cpu, instruction.Operand1);
			int size = instruction.Operand1.Size;

			Write(cpu, (uint)(cpu.ESP.Value - (size / 8)), a, size);

			cpu.ESP.Value = (uint)(cpu.ESP.Value - (size / 8));
		}
	}
}
