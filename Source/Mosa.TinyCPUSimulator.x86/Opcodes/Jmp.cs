// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.TinyCPUSimulator.x86.Opcodes
{
	public class Jmp : BaseX86Opcode
	{
		public override void Execute(CPUx86 cpu, SimInstruction instruction)
		{
			uint v1 = LoadValue(cpu, instruction.Operand1);

			cpu.EIP.Value = v1;
		}

		public override OpcodeFlowType FlowType { get { return OpcodeFlowType.Jump; } }
	}
}