// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.TinyCPUSimulator.x86.Opcodes
{
	public class Iretd : BaseX86Opcode
	{
		public override void Execute(CPUx86 cpu, SimInstruction instruction)
		{
			// Pop Order: EIP, CS, FLAGS
			int size = 32;

			cpu.EIP.Value = Read(cpu, cpu.ESP.Value - (8 * 1), size);

			// Skip CS
			cpu.EFLAGS.Value = Read(cpu, cpu.ESP.Value - (8 * 3), size);

			cpu.ESP.Value = (uint)(cpu.ESP.Value + (size * 3 / 8));
		}

		public override OpcodeFlowType FlowType { get { return OpcodeFlowType.Return; } }
	}
}
