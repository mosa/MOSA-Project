// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.TinyCPUSimulator.x86.Opcodes
{
	public class Popad : BaseX86Opcode
	{
		public override void Execute(CPUx86 cpu, SimInstruction instruction)
		{
			// Pop Order: EDI, ESI, EBP, EBX, EDX, ECX, and EAX
			int size = 32;

			cpu.EDI.Value = Read(cpu, cpu.ESP.Value - (8 * 1), size);
			cpu.ESI.Value = Read(cpu, cpu.ESP.Value - (8 * 2), size);
			cpu.EBP.Value = Read(cpu, cpu.ESP.Value - (8 * 3), size);
			// Skip ESP
			cpu.EBX.Value = Read(cpu, cpu.ESP.Value - (8 * 4), size);
			cpu.EDX.Value = Read(cpu, cpu.ESP.Value - (8 * 5), size);
			cpu.ECX.Value = Read(cpu, cpu.ESP.Value - (8 * 6), size);
			cpu.EAX.Value = Read(cpu, cpu.ESP.Value - (8 * 7), size);

			cpu.ESP.Value = (uint)(cpu.ESP.Value + (size * 8 / 8));
		}
	}
}