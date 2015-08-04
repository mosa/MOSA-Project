// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.TinyCPUSimulator.x86.Opcodes
{
	public class Call : BaseX86Opcode
	{
		public override void Execute(CPUx86 cpu, SimInstruction instruction)
		{
			uint v1 = LoadValue(cpu, instruction.Operand1);
			int size = 32;

			Write(cpu, (uint)(cpu.ESP.Value - (size / 8)), cpu.EIP.Value, size);

			uint target;

			if (instruction.Operand1.IsLabel || instruction.Operand1.IsRegister)
			{
				target = v1;
			}
			else
			{
				target = (uint)(cpu.EIP.Value + (int)v1);
			}

			cpu.ESP.Value = (uint)(cpu.ESP.Value - (size / 8));
			cpu.EIP.Value = target;
		}

		public override OpcodeFlowType FlowType { get { return OpcodeFlowType.Call; } }
	}
}
