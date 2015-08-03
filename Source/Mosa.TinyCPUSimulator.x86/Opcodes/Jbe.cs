// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.TinyCPUSimulator.x86.Opcodes
{
	public class Jbe : BaseX86Opcode
	{
		public override void Execute(CPUx86 cpu, SimInstruction instruction)
		{
			if (cpu.EFLAGS.Carry || cpu.EFLAGS.Zero)
			{
				cpu.EIP.Value = ResolveBranch(cpu, instruction.Operand1);
			}
		}

		public override OpcodeFlowType FlowType { get { return OpcodeFlowType.Branch; } }
	}
}