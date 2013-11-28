﻿/*
 * (c) 2013 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

namespace Mosa.TinyCPUSimulator.x86.Opcodes
{
	public class Jns : BaseX86Opcode
	{
		public override void Execute(CPUx86 cpu, SimInstruction instruction)
		{
			if (!cpu.EFLAGS.Sign)
			{
				cpu.EIP.Value = ResolveBranch(cpu, instruction.Operand1);
			}
		}

		public override OpcodeFlowType FlowType { get { return OpcodeFlowType.Branch; } }
	}
}