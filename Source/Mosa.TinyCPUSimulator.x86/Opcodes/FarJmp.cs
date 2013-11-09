/*
 * (c) 2013 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

namespace Mosa.TinyCPUSimulator.x86.Opcodes
{
	public class FarJmp : BaseX86Opcode
	{
		public override void Execute(CPUx86 cpu, SimInstruction instruction)
		{
			if (instruction.Operand1 == null)
				return;

			uint v1 = LoadValue(cpu, instruction.Operand1);

			cpu.EIP.Value = (uint)(cpu.EIP.Value + (int)v1);
		}

		public override OpcodeFlowType FlowType { get { return OpcodeFlowType.Jump; } }
	}
}