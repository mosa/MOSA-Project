// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.TinyCPUSimulator.x86.Opcodes
{
	public class Cdq : BaseX86Opcode
	{
		public override void Execute(CPUx86 cpu, SimInstruction instruction)
		{
			if (IsSign(cpu.EAX.Value))
				cpu.EDX.Value = ~(uint)0;
			else
				cpu.EDX.Value = 0;
		}
	}
}