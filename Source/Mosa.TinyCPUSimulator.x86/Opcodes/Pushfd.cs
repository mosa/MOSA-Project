// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.TinyCPUSimulator.x86.Opcodes
{
	public class Pushfd : BaseX86Opcode
	{
		public override void Execute(CPUx86 cpu, SimInstruction instruction)
		{
			int size = instruction.Operand1.Size;

			uint r = cpu.EFLAGS.Value & 0x00FCFFFF;

			Write(cpu, cpu.ESP.Value, r, size);

			cpu.ESP.Value = (uint)(cpu.ESP.Value - (size / 8));
		}
	}
}
