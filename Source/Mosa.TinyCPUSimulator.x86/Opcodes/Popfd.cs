// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.TinyCPUSimulator.x86.Opcodes
{
	public class Popfd : BaseX86Opcode
	{
		public override void Execute(CPUx86 cpu, SimInstruction instruction)
		{
			int size = 32;

			cpu.EFLAGS.Value = Read(cpu, cpu.ESP.Value, size);

			cpu.ESP.Value = (uint)(cpu.ESP.Value + (size / 8));
		}
	}
}
